using UnityEngine;

public class NetworkIdentity : MonoBehaviour
{
    [Header("Identificación")]
    public string networkId;
    public bool isLocalPlayer;
    public bool hasAuthority;

    public void Setup(string id, bool local, bool authority)
    {
        networkId = id;
        isLocalPlayer = local;
        hasAuthority = authority;
    }
}
