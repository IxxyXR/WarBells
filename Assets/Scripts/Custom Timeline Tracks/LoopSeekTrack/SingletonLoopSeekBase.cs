using UnityEngine;
using System;
using System.Collections;

public abstract class SingletonLoopSeekBase<T> : MonoBehaviour where T : SingletonLoopSeekBase<T>
{
	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				Type type = typeof(T);

				GameObject[] objs = GameObject.FindGameObjectsWithTag("TagSingleton");

				for (int j = 0; j < objs.Length; j++)
				{
					instance = (T)objs[j].GetComponent(type);
					if (instance != null)
						return instance;
				}

				//Debug.LogWarning(string.Format("{0} is not found", type.Name));
			}

            return instance;
		}
	}
	protected static T instance;

    //[Inspect(0)]
    //[SerializeField]
    //protected bool DontDestroy = false;

    // DestroyされるSingletonでは、OnLevelWasLoaded内の処理をしないようにするための措置
    //protected bool CanUseOnLevelWasLoaded = false;

    //protected void Awake(){}

	protected bool CheckInstance()
	{
		bool found = false;

        if (tag == "TagSingleton")
			found = true;

		if (!found)
		{			
			Debug.Assert(false, "Singleton's tag should be TagSingleton !", transform);
		}

        if (instance == null)
		{
			instance = (T)this;
            return true;
		}
		else if (Instance == this)
		{
            return true;
		}

		Destroy(gameObject);
		return false;
	}
}
