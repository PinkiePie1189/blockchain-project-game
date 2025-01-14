using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Solana.Unity.SDK;
using Solana.Unity.Programs.Utilities;
using Solana.Unity.Rpc.Models;
using Solana.Unity.Rpc.Types;
using Solana.Unity.Wallet;
using Solana.Unity.Programs;
using Solana.Unity.Rpc;
using Solana.Unity.Rpc.Builders;
using UnityEngine;

public class login : MonoBehaviour
{
    // public string programId = "YourProgramPublicKeyHere"; // Replace with your program's public key
    public string idlFileName = "ProgramIdl"; // Name of the IDL JSON file in the Resources folder
    private PublicKey pdaPublicKey; // The derived PDA
    private Account walletAccount; // User's wallet account
    // private AnchorProgram anchorProgram; // Loaded Anchor Program
    private static readonly IRpcClient rpcClient = ClientFactory.GetClient(Cluster.DevNet);

    async void Start()
    {
        /*await Initialize();
        await DerivePDA();
        await CallPingMethod();*/
    }

    private async Task Initialize()
    {
        // Initialize the wallet
        walletAccount = new Account(); // Replace with actual wallet setup logic
        Debug.Log("Wallet initialized with public key: " + walletAccount.PublicKey);

        // Load IDL file from Resources
        TextAsset idlTextAsset = Resources.Load<TextAsset>(idlFileName);
        if (idlTextAsset == null)
        {
            Debug.LogError("Failed to load IDL file.");
            return;
        }

        Debug.Log("IDL successfully loaded and program initialized.");
        
        byte[] methodIdentifier = SHA256.Create()
            .ComputeHash(Encoding.UTF8.GetBytes("global:ping"))
            .Take(8) // Anchor uses the first 8 bytes of the hash
            .ToArray();
    }

    /*private async Task DerivePDA()
    {
        if (string.IsNullOrEmpty(programId))
        {
            Debug.LogError("Program ID is not set.");
            return;
        }

        // Derive PDA
        var seed = Encoding.UTF8.GetBytes("pings"); // Seed as bytes
        var programPublicKey = new PublicKey(programId);

        // Find Program Address
        bool success = PublicKey.TryFindProgramAddress(
            new List<byte[]> { seed },
            programPublicKey,
            out PublicKey pda,
            out byte bump
            
        );

        pdaPublicKey = pda;

        Debug.Log("Successfully derive PDA?" + success);
        Debug.Log("Derived PDA: " + pdaPublicKey);
        Debug.Log("Derived bump: " + bump);
    }*/

    /*private async Task CallPingMethod()
    {
        if (anchorProgram == null || pdaPublicKey == null)
        {
            Debug.LogError("Anchor program or PDA is not initialized.");
            return;
        }

        // Define the accounts for the transaction
        var accounts = new List<AccountMeta>
        {
            AccountMeta.Writable(pdaPublicKey, false), // PDA account
            AccountMeta.Writable(walletAccount.PublicKey, true), // User's wallet (payer)
        };

        try
        {
            // Call the "ping" method
            string methodName = "ping";
            var transaction = await anchorProgram.Request(
                methodName,
                accounts,
                walletAccount // Signer
            );

            // Log the transaction signature
            Debug.Log("Ping transaction submitted with signature: " + transaction.Signatures[0].Signature);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error calling ping method: " + ex.Message);
        }
    }
    */


    public async void Login()
    {

        var pubkey = new PublicKey("9J8ntNdbFkicKQrTiJi2HXNW2PkCevdWZp8CMd5Zubdu");
        var privkey = new PrivateKey(new byte[]
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
        }); // placeholder
        
        
        var account = new Account(privkey.KeyBytes, pubkey.KeyBytes);

        var programId = new PublicKey("8fSxs8gMJK58c1sXfdMCbGq5EDj5X2Tbin8WerjLYkzD");
        
        
        byte[] methodIdentifier = SHA256.Create()
            .ComputeHash(Encoding.UTF8.GetBytes("global:get_pings"))
            .Take(8) // Anchor uses the first 8 bytes of the hash
            .ToArray();
        
        // Derive PDA
        var seed = Encoding.UTF8.GetBytes("pings"); // Seed as bytes

        // Find Program Address
        bool success = PublicKey.TryFindProgramAddress(
            new List<byte[]> { seed },
            programId,
            out PublicKey pda,
            out byte bump
            
        );
        
        Debug.Log(pda.ToString());

        // 3. Define accounts for the instruction
        var accounts = new List<AccountMeta>
        {
            
            AccountMeta.Writable(pda, isSigner: false),// Wallet account
            AccountMeta.ReadOnly(pubkey, isSigner: true),
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
            .Build(account);

        // 8. Send the transaction
        var result = await rpcClient.SendTransactionAsync(transaction);

        // 9. Log the result
        if (result.WasSuccessful)
        {
            Debug.Log($"Transaction successful! Signature: {result.Result}");
            Debug.Log($"Result data: {result.Result}" );
            
            while (true) {
                var transactionDetails = await rpcClient.GetTransactionAsync(result.Result);
                if (transactionDetails.WasSuccessful)
                {
                    Debug.Log("Transaction confirmed successfully!");

                    foreach (var log in transactionDetails.Result.Meta.LogMessages) {
                        Debug.Log($"Log: {log}");
                    }
                    Debug.Log($"Transaction Details: {transactionDetails.Result.Meta.LogMessages}");
                    break;
                }

                Debug.LogWarning($"Failed to fetch transaction details: {transactionDetails.Reason}");
                
                Thread.Sleep(10000);
            }
        }
        else
        {
            Debug.LogError($"Transaction failed: {result.Reason}");
        }
        
        
    }
}