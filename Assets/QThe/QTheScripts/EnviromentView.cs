using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Enviroment
{
    public EnviromentType enviromentType;
    public GameObject enviromentObj;
}

public class EnviromentView : MonoBehaviour
{
    [SerializeField] private List<Enviroment> enviromentList = new List<Enviroment>();
    public void SetEnviroment(EnviromentType targetType)
    {
        foreach (var envi in enviromentList)
        {
            // set active all enviroment to false and target enviroment to true
            if (envi.enviromentObj != null)
            {
                bool isActive = (envi.enviromentType == targetType);
                envi.enviromentObj.SetActive(isActive);
            }
        }
    }
}
