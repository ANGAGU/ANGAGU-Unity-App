using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class AssetBundleLoader : MonoBehaviour
{
    // Start is called before the first frame update

    private string modelName = "testModel/objbed";
    GameObject modelObject;
    IEnumerator Start()
    {

        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle("http://d3u3zwu9bmcdht.cloudfront.net/" + modelName);

        // www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
        Debug.Log("http://d3u3zwu9bmcdht.cloudfront.net/" + modelName);

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            AssetBundle assetFile = DownloadHandlerAssetBundle.GetContent(www);
            if (assetFile == null)
            {
                Debug.Log("Asset Load Error");
            }
            AssetBundleRequest prefab = assetFile.LoadAssetAsync("IKE050020");
            yield return prefab;
            modelObject = prefab.asset as GameObject;
        }


        /**** init for .dae file ****/
        GameObject daeObject = Instantiate(modelObject);
        // 태그 설정을 위해선 해당 태그 이름을 먼저 만들어 줘야한다.
        // 만약 동적인 tag가 필요하다면 editor script가 필요하다.
        daeObject.transform.tag = "Model";

        // destroy camera object in .dae
        // Destroy(daeObject.transform.GetChild(0).gameObject);
        daeObject.AddComponent<sizeInit>();


    }

    // Update is called once per frame
    void Update()
    {

    }
}
