using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BucketManagerView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinTxt;
    private readonly IGameStorage storage = new PlayerPrefsStorage();
    private int bucket;

    public int GetBucket()
    {
        bucket = storage.GetInt("Bucket", 0);
        return bucket;
    }

    public void AddBucket(int price)
    {
        bucket += price;
        SetBucketPref(bucket);
    }

    public void SendBucket(int price)
    {
        bucket -= price;
        SetBucketPref(bucket);
    }

    private void SetBucketPref(int bucket)
    {
        storage.SetInt("Bucket", bucket);
        storage.Save();
    }

    public void UpdateCoinUI(int bucket)
    {
        coinTxt.text = bucket.ToString();
    }
}
