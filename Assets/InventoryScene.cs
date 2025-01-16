using System;
using System.Buffers.Text;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Solana.Unity.Rpc.Models;
using Solana.Unity.Rpc.Types;
using Solana.Unity.Rpc;
using Solana.Unity.SDK.Nft;
using Solana.Unity.Wallet;
using Solana.Unity.Wallet.Utilities;
using UnityEngine.UI;


public class BaseAssetV1
{
    public byte[] Key { get; set; } // Corresponds to `Key`
    public byte[] Owner { get; set; } // Corresponds to `Pubkey`
    public string UpdateAuthority { get; set; } // Corresponds to `UpdateAuthority`
    public string Name { get; set; } // Corresponds to `String`
    public string Uri { get; set; } // Corresponds to `String`
    public ulong? Seq { get; set; } // Corresponds to `Option<u64>`

    public static BaseAssetV1 DecodeFromBytes(byte[] rawData)
    {
        using (var reader = new BinaryReader(new MemoryStream(rawData)))
        {
            var asset = new BaseAssetV1();

            // Decode Key (fixed size, assuming 32 bytes for simplicity)
            asset.Key = reader.ReadBytes(1);

            // Decode Owner (fixed size, assuming 32 bytes for Pubkey)
            asset.Owner = reader.ReadBytes(32);

            // Decode UpdateAuthority (fixed size, assuming 32 bytes for UpdateAuthority)

            var encoder = new Base58Encoder();
            asset.UpdateAuthority = encoder.EncodeData(reader.ReadBytes(33).Skip(1).ToArray());

            // Decode Name (length-prefixed string)
            int nameLength = reader.ReadInt32();
            asset.Name = Encoding.UTF8.GetString(reader.ReadBytes(nameLength));

            // Decode Uri (length-prefixed string)
            int uriLength = reader.ReadInt32();
            asset.Uri = Encoding.UTF8.GetString(reader.ReadBytes(uriLength));

            // Decode Seq (Option<u64>)
            bool hasSeq = reader.ReadByte() == 1; // Assuming 1 byte for Option presence (0 = None, 1 = Some)
            if (hasSeq)
            {
                asset.Seq = reader.ReadUInt64();
            }
            else
            {
                asset.Seq = null;
            }

            return asset;
        }
    }
}

public class InventoryScene : MonoBehaviour
{

    public string address = "9J8ntNdbFkicKQrTiJi2HXNW2PkCevdWZp8CMd5Zubdu";

    public string mplCoreProgram = "CoREENxT6tW1HoK8ypY1SxRMZTcVPm7R94rH4PZNhX7d";

    public string ownerPda = "4cEoKcrXFpWsU4hXW1GEXkDZratmQ3tRB9iqj6oo8CPg";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public GameObject itemPrefab;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    


    public async void init()
    {
        Debug.Log("aici");
        IRpcClient rpc = ClientFactory.GetClient(Cluster.DevNet);

        var accounts = await rpc.GetProgramAccountsAsync(
            "CoREENxT6tW1HoK8ypY1SxRMZTcVPm7R94rH4PZNhX7d",
            Commitment.Finalized,
            null,
            new List<MemCmp>
            {
                new MemCmp
                {
                    Offset = 0,
                    Bytes = "2"
                },
                new MemCmp
                {
                    Offset = 1,
                    Bytes = address
                }
            });
        // Debug.Log(accounts.Result?.ToArray());

        if (accounts.WasSuccessful)
        {
            //var res = accounts.Result?.ToArray();
            Debug.Log("lol");
            var result = accounts.Result;

            foreach (var account in result) {
                
                Debug.Log(account.PublicKey);

                byte[] rawData = Convert.FromBase64String(account.Account.Data[0]);
                
                var nft = BaseAssetV1.DecodeFromBytes(rawData);
                
                if (nft.UpdateAuthority.Equals(ownerPda)) {
                    
                    var queryString = new Uri(nft.Uri).Query;
                    var queryDictionary = System.Web.HttpUtility.ParseQueryString(queryString);
                    
                    var upgradeString  = queryDictionary.Get("upgrade");

                    if (!string.IsNullOrEmpty(upgradeString)) {
                        var newObject = Instantiate(itemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                        newObject.transform.SetParent(transform);
                        newObject.transform.position = transform.position;
                        newObject.GetComponent<InventoryItem>().itemName = nft.Name;
                        newObject.GetComponent<InventoryItem>().itemUpgrade = Int32.Parse(queryDictionary.Get("upgrade"));
                        newObject.GetComponent<InventoryItem>().updateItem();
                    }
                    
                    
                   
                }
            }
        }
    }
}
