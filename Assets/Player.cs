using Solana.Unity.SDK;
using UnityEngine;



public class Player : MonoBehaviour
{
    private WalletBase wallet;

    // Use this for initialization
    private async void Start()
    {
        // Call the Login function to authenticate the wallet
        Web3.Instance.LoginWithWalletAdapter();

        // Access the logged-in wallet

        /*SolanaWalletAdapterOptions options = new SolanaWalletAdapterOptions()
        wallet = new SolanaWalletAdapter(walletAdapterOptions, RpcCluster.DevNet, ...);*/


        if (wallet != null)
        {
            Debug.Log("Wallet connected: " + wallet.Account.PublicKey);
        }
        else
        {
            Debug.LogError("Failed to connect wallet.");
        }
    }

    /*public async void GetBalance()
    {
        if (wallet == null)
        {
            Debug.LogError("No wallet connected. Please login first.");
            return;
        }

        // Fetch the wallet balance using the RPC
        var rpc = Web3.Instance.Rpc;
        var publicKey = wallet.Account.PublicKey;
        var balanceResult = await rpc.GetBalanceAsync(publicKey);

        if (balanceResult.WasSuccessful)
        {
            Debug.Log($"Wallet Balance: {balanceResult.Result.Value} lamports");
        }
        else
        {
            Debug.LogError($"Failed to fetch balance: {balanceResult.Reason}");
        }
    }*/
}
