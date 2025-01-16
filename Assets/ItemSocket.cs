using System.Linq;
using Solana.Unity.Programs;
using Solana.Unity.Rpc;
using Solana.Unity.SDK.Nft;
using Solana.Unity.Soar.Accounts;
using Solana.Unity.Wallet;
using UnityEngine;
using UnityEngine.UI;
using Solana.Unity.Rpc;
using Solana.Unity.Rpc.Builders;
using Solana.Unity.Rpc.Types;

public class ItemSocket : MonoBehaviour
{


    public PublicKey equippedNftAddress;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IRpcClient rpc = ClientFactory.GetClient(Cluster.DevNet);
        var result = rpc.GetTokenAccountsByOwnerAsync(
                "9J8ntNdbFkicKQrTiJi2HXNW2PkCevdWZp8CMd5Zubdu",
                null,
                TokenProgram.ProgramIdKey,
                Commitment.Confirmed).Result.Result.Value.ToArray();

        foreach (var token in result)
        {
            Debug.Log(token.PublicKey);
        }
        // Nft.TryGetNftData("DLzTay33woq4qxHi3eBHSE35aU6m2sMQH9SJxbYa44pZ", )
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Equip(Sprite sprite, PublicKey nftAddress)
    {
        Debug.Log("aici");
        equippedNftAddress = nftAddress;

        GetComponent<Image>().sprite = sprite;

    }
}
