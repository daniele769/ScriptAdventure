using Godot;
using HiveMQtt.Client;
using HiveMQtt.Client.Options;
using HiveMQtt.Client.Results;
using System;
using System.Linq;

public static partial class Static_BrokerConnection
{
    private static HiveMQClient GetClient(BrokerChatter brokerChatter, ref bool isClientOk)
	{
		var option = new HiveMQClientOptions();
		option.Host = PlayerPrefs.GetString(Costanti.ipBroker);
		option.UserName = PlayerPrefs.GetString(Costanti.usernameBroker);
		option.Password = PlayerPrefs.GetString(Costanti.passwordBroker);

        try
        {
            option.Port = PlayerPrefs.GetString(Costanti.portBroker).ToInt();
        }
        catch 
        {
            isClientOk = false;
            brokerChatter.SetTextError("Porta Broker MQTT non valida");
        }

		return new HiveMQClient(option);
		
	}

    public static async void SendMessageToBroker(BrokerChatter brokerChatter, string topic, string text)
    {
        if(!PlayerPrefs.ListKeys.ToList<string>().Contains(Costanti.isBrokerConnectionOn) || 
            !PlayerPrefs.GetBool(Costanti.isBrokerConnectionOn))
        {
            GD.Print("Connessione Broker disabilitata");
            return;
        }

        if(!IsBrokerSetted())
        {
            GD.Print("Nessun Broker settato nelle impostazioni");
            return;
        }
            

        bool isClientOk  = true;       
        HiveMQClient client = GetClient(brokerChatter, ref isClientOk);
        GD.Print("Client creato"); 
        if(!isClientOk)
        {
            GD.Print("Client non valido");
            brokerChatter.SetConnectionResult(false);
            return;
        } 
        
        GD.Print("Tento la connessione al Broker");
        ConnectResult connectResult;

        try 
        {
            connectResult = await client.ConnectAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            GD.Print("Connessione al Broker fallita");
            brokerChatter.SetConnectionResult(false);
            brokerChatter.SetTextError(ex.Message);
            return;
        }

        if(connectResult.ReasonCode == HiveMQtt.MQTT5.ReasonCodes.ConnAckReasonCode.Success)
        {
            GD.Print("Connessione al Broker avvenuta con successo");
            await client.PublishAsync(topic, text).ConfigureAwait(false);
            GD.Print("Messaggio inviato al Broker con successo");
            await client.DisconnectAsync();
            GD.Print("Disconnessione dal Broker effettuata");
            return;
        }
        GD.Print("Connessione al Broker fallita");
        brokerChatter.SetConnectionResult(false);
        brokerChatter.SetTextError("Controlla i dati del Broker nelle impostazioni");

    }

    private static bool IsBrokerSetted()
    {
        bool exist = false;
        foreach(string key in PlayerPrefs.ListKeys)
        {
            if(key.Equals(Costanti.ipBroker) || key.Equals(Costanti.portBroker) || key.Equals(Costanti.usernameBroker) || 
               key.Equals(Costanti.passwordBroker))
            {
                exist = true;
            }
        }

        if(exist)
        {
            if(PlayerPrefs.GetString(Costanti.ipBroker).Equals("") && PlayerPrefs.GetString(Costanti.usernameBroker).Equals("") &&
               PlayerPrefs.GetString(Costanti.passwordBroker).Equals("") && PlayerPrefs.GetString(Costanti.portBroker).Equals(""))
            {
                exist = false;
            }
        }

        return exist;
    }
}
