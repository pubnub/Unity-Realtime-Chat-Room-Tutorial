using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubNubAPI;
using UnityEngine.UI;

public class JSONInformation
{
    public string username;
    public string text;
}

public class SendMessage : MonoBehaviour {
    public static PubNub pubnub;
    public Font customFont;
    public Button SubmitButton;
    public Canvas canvasObject;
    public InputField UsernameInput;
    public InputField TextInput;
    public int indexcounter = 0;
    public Text deleteText;
    public Text moveTextUpwards;
    private Text text;

    float paddingX = -10F;
    float paddingY = 300F;
    float padding = 550F;
    float height = 30;
    ushort maxMessagesToDisplay = 12;

    string channel = "chatchannel3";

    // Create a chat message queue so we can interate through all the messages
    Queue<GameObject> chatMessageQueue = new Queue<GameObject>();
   
    void Start()
    {
        // Use this for initialization
        PNConfiguration pnConfiguration = new PNConfiguration();
        pnConfiguration.PublishKey = "demo";
        pnConfiguration.SubscribeKey = "demo";
        pnConfiguration.LogVerbosity = PNLogVerbosity.BODY;
        pnConfiguration.UUID = System.Guid.NewGuid().ToString();
        pubnub = new PubNub(pnConfiguration);

        // Add Listener to Submit button to send messages
        Button btn = SubmitButton.GetComponent<Button>();
        
        btn.onClick.AddListener(TaskOnClick);

        // Fetch the maxMessagesToDisplay messages sent on the given PubNub channel
        pubnub.FetchMessages()
            .Channels(new List<string> { channel })
            .Count(maxMessagesToDisplay)
            .Async((result, status) =>
            {
            if (status.Error)
            {
                Debug.Log(string.Format(
                    " FetchMessages Error: {0} {1} {2}", 
                    status.StatusCode, status.ErrorData, status.Category
                ));
            }
            else
            {
                foreach (KeyValuePair<string, List<PNMessageResult>> kvp in result.Channels)
                {
                    foreach (PNMessageResult pnMessageResult in kvp.Value)
                    {
                        // Format data into readable format
                        JSONInformation chatmessage = JsonUtility.FromJson<JSONInformation>(pnMessageResult.Payload.ToString());

                        // Call the function to display the message in plain text
                        CreateChat(chatmessage);
                    }
                 }
             }
             });

        // This is the subscribe callback function where data is recieved that is sent on the channel
        pubnub.SubscribeCallback += (sender, e) =>
        {
            SubscribeEventEventArgs message = e as SubscribeEventEventArgs;
            if (message.MessageResult != null)
            {
                // Format data into a readable format
                JSONInformation chatmessage = JsonUtility.FromJson<JSONInformation>(message.MessageResult.Payload.ToString());
                
                // Call the function to display the message in plain text
                CreateChat(chatmessage);

                // When a new chat is created, remove the first chat and transform all the messages on the page up
                SyncChat();
            }
        };
        
        // Subscribe to a PubNub channel to receive messages when they are sent on that channel
        pubnub.Subscribe()
            .Channels(new List<string>() {
                channel
            })
            .WithPresence()
            .Execute();
        
    }

    // Function used to create new chat objects based of the data received from PubNub
    void CreateChat(JSONInformation payLoad){

        // Create a string with the username and text
        string currentObject = string.Concat(payLoad.username, payLoad.text);

        // Create a new gameobject that will display text of the data sent via PubNub
        GameObject chatMessage = new GameObject(currentObject);
        chatMessage.transform.SetParent(canvasObject.GetComponent<Canvas>().transform);
        chatMessage.transform.localPosition = Vector3.zero;
        chatMessage.transform.position = new Vector3(canvasObject.transform.position.x - paddingX, canvasObject.transform.position.y - paddingY + padding - (indexcounter * height), 1F);        
        chatMessage.AddComponent<Text>().text = currentObject;

        // Assign text to the gameobject. Add visual properties to text
        var chatText = chatMessage.GetComponent<Text>();
        chatText.font = customFont;
        chatText.color = UnityEngine.Color.blue;
        chatText.fontSize = 25;
        
        // Assign a RectTransform to gameobject to maniuplate positioning of chat.
        RectTransform rectTransform;
        rectTransform = chatText.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(435, height);
        
        // Assign the gameobject to the queue of chatmessages
        chatMessageQueue.Enqueue(chatMessage);
                       
        // Keep track of how many objects we have displayed on the screen
        indexcounter++;
    }

    void SyncChat() {
        // If more maxMessagesToDisplay objects are on the screen, we need to start removing them
        if (indexcounter > maxMessagesToDisplay)
        {
            // Delete the first gameobject in the queue
            GameObject deleteChat = chatMessageQueue.Dequeue();
            Destroy(deleteChat);

            // Move all existing text gameobjects up the Y axis 
            int c = 0;
            foreach (GameObject moveChat in chatMessageQueue)
            {
                RectTransform moveText = moveChat.GetComponent<RectTransform>();
                moveText.position = new Vector3(canvasObject.transform.position.x - paddingX, canvasObject.transform.position.y - paddingY + padding - (c * height), 1F);
                c++;
            }
        }
    }

	// Update is called once per frame
	void Update () {
		
	}

    void TaskOnClick()
    {
        // When the user clicks the Submit button,
        // create a JSON object from input field input
        JSONInformation publishMessage = new JSONInformation();
        publishMessage.username = string.Concat(UsernameInput.text, ": ");
        publishMessage.text = TextInput.text;
        string publishMessageToJSON = JsonUtility.ToJson(publishMessage);

        // Publish the JSON object to the assigned PubNub Channel
        pubnub.Publish()
            .Channel(channel)
            .Message(publishMessageToJSON)
            .Async((result, status) =>
            {
                if (status.Error)
                {
                    Debug.Log(status.Error);
                    Debug.Log(status.ErrorData.Info);
                }
                else
                {
                    Debug.Log(string.Format("Publish Timetoken: {0}", result.Timetoken));
                }
            });

        TextInput.text = "";
    }
}
