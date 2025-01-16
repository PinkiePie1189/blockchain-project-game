using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Chaos.NaCl;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Security;
using Solana.Unity.Rpc;
using Solana.Unity.Rpc.Builders;
using Solana.Unity.Rpc.Models;
using Solana.Unity.SDK;
using Solana.Unity.Wallet;
using UnityEditor.VersionControl;
using UnityEngine;

using Chaos.NaCl;
using Solana.Unity.Programs;
using Solana.Unity.Wallet.Utilities;
using PublicKey = Solana.Unity.Wallet.PublicKey;

public class BlacksmithScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    Account GenerateKeypair()
    {
        // Generate a random seed
        byte[] seed = new byte[32];
        using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
        {
            rng.GetBytes(seed);
        }

        // Generate the keypair using the seed
        byte[] privateKey = Ed25519.ExpandedPrivateKeyFromSeed(seed);
        byte[] publicKey = new byte[32];
        Array.Copy(privateKey, 32, publicKey, 0, 32); // Public key is derived from the expanded private key

        // Display the keys
        Debug.Log("Private Key (Base64): " + Convert.ToBase64String(privateKey));
        Debug.Log("Public Key (Base64): " + Convert.ToBase64String(publicKey));

        var encoder = new Base58Encoder();

        return new Account(privateKey, publicKey);
    }

    public async void requestItem()
    {

        var rpcClient = ClientFactory.GetClient(Cluster.DevNet);
        var pubkey = new PublicKey("9J8ntNdbFkicKQrTiJi2HXNW2PkCevdWZp8CMd5Zubdu");
        var privkey = new PrivateKey(new byte[]
        {
            157,66,149,167,75,41,44,219,234,90,48,209,194,132,193,6,209,244,180,3,134,252,8,175,88,99,65,212,28,99,61,134,123,65,45,186,140,210,164,137,48,162,147,131,152,108,26,132,145,106,17,164,123,17,143,77,199,46,207,202,139,206,47,196
        }); // placeholder
        
        
        var account = new Account(privkey.KeyBytes, pubkey.KeyBytes);

        var programId = new PublicKey("4vbkSNKb9hx4DVe1md2CBzLwLwE8xsKAwBALe8CrNxVx");
        
        
        byte[] methodIdentifier = SHA256.Create()
            .ComputeHash(Encoding.UTF8.GetBytes("global:request_item"))
            .Take(8) // Anchor uses the first 8 bytes of the hash
            .ToArray();
        
        // Derive PDA
        var seed = Encoding.UTF8.GetBytes("user_pda"); // Seed as bytes
        // Find Program Address
        bool success = PublicKey.TryFindProgramAddress(
            new List<byte[]> { seed },
            programId,
            out PublicKey userPda,
            out byte bumpUserPda
            
        );

        bool succes = PublicKey.TryFindProgramAddress(
            new List<byte[]> { Encoding.UTF8.GetBytes("owner_pda") },
            programId,
            out PublicKey ownerPda,
            out byte bumpOwnerPda);
        
        Debug.Log(userPda.ToString());
        Debug.Log(ownerPda.ToString());

        // 3. Define accounts for the instruction


        var key  = GenerateKeypair();
        Debug.Log(key);
        
        var accounts = new List<AccountMeta>
        {
            AccountMeta.Writable(userPda, isSigner: false),
            AccountMeta.Writable(pubkey, isSigner: true),
            AccountMeta.Writable(ownerPda, isSigner: false),
            AccountMeta.ReadOnly(new PublicKey("CoREENxT6tW1HoK8ypY1SxRMZTcVPm7R94rH4PZNhX7d"), isSigner: false),
            AccountMeta.Writable(key.PublicKey, isSigner: true),
            AccountMeta.ReadOnly(new PublicKey("11111111111111111111111111111111"), isSigner: false)
        };

        // 4. Build the instruction
        var instruction = new TransactionInstruction
        {
            ProgramId = programId,
            Keys = accounts,
            Data = methodIdentifier
        };

        // 5. Get the recent blockhash
        var blockhash = await rpcClient.GetLatestBlockHashAsync();

        // 6. Create the transaction
        var transaction = new TransactionBuilder()
            .SetFeePayer(account)
            .SetRecentBlockHash(blockhash.Result.Value.Blockhash)
            .AddInstruction(instruction)
            .Build(new List<Account>
            {
              account,
              key
            });

        Debug.Log(System.Text.Encoding.UTF8.GetString(transaction));

        var result = await rpcClient.SendTransactionAsync(transaction);

        if (result.WasSuccessful)
        {
            Debug.Log(result.Result);
        }
        else
        {
            Debug.Log(result.Reason);
        }
    }
}
