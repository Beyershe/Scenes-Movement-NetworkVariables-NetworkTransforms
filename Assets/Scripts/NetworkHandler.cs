using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Unity.Netcode;
using System;

public class NetworkHandler : NetworkBehaviour
{
    void Start()
    {
        NetworkManager.OnClientStarted += OnClientStarted;
        NetworkManager.OnServerStarted += OnServerStarted;

    }

    private bool hasPrinted = false;

    private void PrintMe()
    {
        if(hasPrinted)
        {
            return;
        }
        Debug.Log("I AM");
        hasPrinted = true;
        if (IsServer)
        {
            Debug.Log($" the Server {NetworkManager.ServerClientId}");
        }
        if (IsHost)
        {
            Debug.Log($" the Host {NetworkManager.ServerClientId}/{NetworkManager.LocalClientId}");
        }
        if (IsClient)
        {
            Debug.Log($" the Client {NetworkManager.LocalClientId}");
        }
        if (!IsServer && !IsClient)
        {
            Debug.Log(" nothing yet");
            hasPrinted = false;
        }
    }

    private void OnClientStarted()
    {
        Debug.Log("Client Started");
        NetworkManager.OnClientConnectedCallback += ClientOnClientConnected;
        NetworkManager.OnClientDisconnectCallback += ClientOnClientDisconnected;
        NetworkManager.OnClientStopped += ClientOnClientStopped;
        PrintMe();
    }

    //Client Actions

    private void ClientOnClientConnected(ulong clientId) 
    {
        PrintMe();
        Debug.Log($"I {clientId} have connected to the server");
    }
    private void ClientOnClientDisconnected(ulong clientId) 
    {
        Debug.Log($"I {clientId} have disconnected from the server");
    }
    private void ClientOnClientStopped(ulong clientId)
    {
        Debug.Log("Client Stopped");
        hasPrinted = false;
        NetworkManager.OnClientConnectedCallback -= ClientOnClientConnected;
        NetworkManager.OnClientDisconnectCallback -= ClientOnClientDisconnected;
        NetworkManager.OnClientStopped -= ClientOnClientStopped;
    }

    private void ClientOnClientStopped(bool obj)
    {
        throw new NotImplementedException();
    }


    // Server Actions
    private void OnServerStarted()
    {
        Debug.Log("Server Started");
        NetworkManager.OnClientConnectedCallback += ServerOnClientConnected;
        NetworkManager.OnClientDisconnectCallback += ServerOnClientDisconnected;
        NetworkManager.OnServerStopped += ServerOnServerStopped;
        PrintMe();
    }

    private void ServerOnClientConnected(ulong clientId) 
    { 
        Debug.Log($"Client {clientId} connected to the server");
    }
    private void ServerOnClientDisconnected(ulong clientId) 
    {
        Debug.Log($"Client {clientId} disconnected to the server");
    }
    private void ServerOnServerStopped(bool indicator) 
    {
        Debug.Log("Server Stopped");
        hasPrinted = false;
        NetworkManager.OnClientConnectedCallback -= ServerOnClientConnected;
        NetworkManager.OnClientDisconnectCallback -= ServerOnClientDisconnected;
        NetworkManager.OnServerStopped -= ServerOnServerStopped;
    }

}
