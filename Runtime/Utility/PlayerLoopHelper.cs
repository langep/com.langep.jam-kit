using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

namespace Langep.JamKit.Utility
{
	
	/// <summary>
	/// Utility class for interacting with UnityEngine.LowLevel.PlayerLoop which
	/// controls the update order of components in Unity.
	/// </summary>
    public static class PlayerLoopHelper
    {
        public static void SetDefaultPlayerLoopSystem()
		{
			PlayerLoop.SetPlayerLoop(PlayerLoop.GetDefaultPlayerLoop());
		}
        
        /// <summary>
        /// Check if a system with type playerLoopSystemType is registered under targetLoopSystemType subsystem.  
        /// </summary>
        public static bool HasPlayerLoopSystem(Type playerLoopSystemType, Type targetLoopSystemType = null)
        {
	        if (playerLoopSystemType == null) return false;
	        var checkTargetSubsystem = targetLoopSystemType != null;

	        var loopSystem = PlayerLoop.GetCurrentPlayerLoop();
	        for (int i = 0, subSystemCount = loopSystem.subSystemList.Length; i < subSystemCount; ++i)
	        {
		        var subSystem = loopSystem.subSystemList[i];
		        if (checkTargetSubsystem && subSystem.type != targetLoopSystemType) continue;
		        
		        var subSubSystems = new List<PlayerLoopSystem>(subSystem.subSystemList);
		        for (var j = 0; j < subSubSystems.Count; ++j)
		        {
			        if (subSubSystems[j].type == playerLoopSystemType) return true;
		        }
	        }

	        return false;
        }        

        /// <summary>
        /// Add updateFunction as playerLoopSystemType under targetLoopSystemType at the specified position.
        /// </summary>
		public static bool AddPlayerLoopSystem(Type playerLoopSystemType, Type targetLoopSystemType, PlayerLoopSystem.UpdateFunction updateFunction, int position = -1)
		{
			if (playerLoopSystemType == null || targetLoopSystemType == null || updateFunction == null) return false;

			var loopSystem = PlayerLoop.GetCurrentPlayerLoop();
			for (int i = 0, subSystemCount = loopSystem.subSystemList.Length; i < subSystemCount; ++i)
			{
				var subSystem = loopSystem.subSystemList[i];
				if (subSystem.type != targetLoopSystemType) continue;
				
				var targetSystem = new PlayerLoopSystem();
				targetSystem.type = playerLoopSystemType;
				targetSystem.updateDelegate = updateFunction;

				var subSubSystems = new List<PlayerLoopSystem>(subSystem.subSystemList);
				if (position >= 0)
				{
					if (position > subSubSystems.Count) throw new ArgumentOutOfRangeException(nameof(position));

					subSubSystems.Insert(position, targetSystem);
					Debug.LogWarningFormat("Added Player Loop System: {0} to: {1} position: {2}/{3} method: {4}", playerLoopSystemType.Name, subSystem.type.Name, position, subSubSystems.Count - 1, updateFunction.Method.Name);
				}
				else
				{
					subSubSystems.Add(targetSystem);
					Debug.LogWarningFormat("Added Player Loop System: {0} to: {1} position: {2}/{2} method: {3}", playerLoopSystemType.Name, subSystem.type.Name, subSubSystems.Count - 1, updateFunction.Method.Name);
				}

				subSystem.subSystemList = subSubSystems.ToArray();
				loopSystem.subSystemList[i] = subSystem;

				PlayerLoop.SetPlayerLoop(loopSystem);

				return true;
			}

			Debug.LogErrorFormat("Failed to add Player Loop System: {0} to: {1}", playerLoopSystemType.Name, targetLoopSystemType.Name);

			return false;
		}

        
        /// <summary>
        /// Add updateFunction as playerLoopSystemType under targetLoopSystemType after updateFunctionAfter
        /// </summary>
		public static bool AddPlayerLoopSystem(Type playerLoopSystemType, Type targetSubSystemType, PlayerLoopSystem.UpdateFunction updateFunctionBefore, PlayerLoopSystem.UpdateFunction updateFunctionAfter)
		{
			if (playerLoopSystemType == null || targetSubSystemType == null || (updateFunctionBefore == null && updateFunctionAfter == null)) return false;

			var loopSystem = PlayerLoop.GetCurrentPlayerLoop();
			for (int i = 0, subSystemCount = loopSystem.subSystemList.Length; i < subSystemCount; ++i)
			{
				var subSystem = loopSystem.subSystemList[i];
				for (int j = 0, subSubSystemCount = subSystem.subSystemList.Length; j < subSubSystemCount; ++j)
				{
					var subSubSystem = subSystem.subSystemList[j];
					if (subSubSystem.type != targetSubSystemType) continue;
					
					var subSubSystems = new List<PlayerLoopSystem>(subSystem.subSystemList);
					var currentPosition = j;

					if (updateFunctionBefore != null)
					{
						var playerLoopSystem = new PlayerLoopSystem();
						playerLoopSystem.type = playerLoopSystemType;
						playerLoopSystem.updateDelegate = updateFunctionBefore;

						subSubSystems.Insert(currentPosition, playerLoopSystem);

						Debug.LogWarningFormat("Added Player Loop System: {0} to: {1} before: {2} method: {3}", playerLoopSystemType.Name, subSystem.type.Name, subSubSystem.type.Name, updateFunctionBefore.Method.Name);

						++currentPosition;
					}

					if (updateFunctionAfter != null)
					{
						++currentPosition;

						var playerLoopSystem = new PlayerLoopSystem();
						playerLoopSystem.type = playerLoopSystemType;
						playerLoopSystem.updateDelegate = updateFunctionAfter;

						subSubSystems.Insert(currentPosition, playerLoopSystem);

						Debug.LogWarningFormat("Added Player Loop System: {0} to: {1} after: {2} method: {3}", playerLoopSystemType.Name, subSystem.type.Name, subSubSystem.type.Name, updateFunctionAfter.Method.Name);
					}

					subSystem.subSystemList = subSubSystems.ToArray();
					loopSystem.subSystemList[i] = subSystem;

					PlayerLoop.SetPlayerLoop(loopSystem);

					return true;
				}
			}

			Debug.LogErrorFormat("Failed to add Player Loop System: {0}", playerLoopSystemType.Name);

			return false;
		}

        /// <summary>
        /// Remove the subsubsystem of type playerLoopSystemType.
        /// </summary>
        public static bool RemovePlayerLoopSystems(Type playerLoopSystemType, Type targetLoopSystemType = null)
		{
			if (playerLoopSystemType == null) return false;
			var checkTargetSubsystem = targetLoopSystemType != null;

			var setPlayerLoop = false;

			var loopSystem = PlayerLoop.GetCurrentPlayerLoop();
			for (int i = 0, subSystemCount = loopSystem.subSystemList.Length; i < subSystemCount; ++i)
			{
				var subSystem = loopSystem.subSystemList[i];
				if (checkTargetSubsystem && subSystem.type != targetLoopSystemType) continue;

				var removedFromSubSystem = false;
				var subSubSystems = new List<PlayerLoopSystem>(subSystem.subSystemList);
				for (var j = subSubSystems.Count - 1; j >= 0; --j)
				{
					if (subSubSystems[j].type != playerLoopSystemType) continue;
					subSubSystems.RemoveAt(j);
					removedFromSubSystem = true;
					Debug.LogWarningFormat("Removed Loop System: {0} from: {1}", playerLoopSystemType.Name, subSystem.type.Name);
				}

				if (removedFromSubSystem != true) continue;
				setPlayerLoop = true;

				subSystem.subSystemList = subSubSystems.ToArray();
				loopSystem.subSystemList[i] = subSystem;
			}

			if (setPlayerLoop == true)
			{
				PlayerLoop.SetPlayerLoop(loopSystem);
			}

			return setPlayerLoop;
		}
    }
}