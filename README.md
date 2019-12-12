# Unity Realtime Chat Room Tutorial

In this tutorial, we'll walk through how to build a basic multiplayer lobby chat room with the Unity 3D game engine. This tutorial will work across iOS, Android, Web and Consoles, so no matter what device you are exporting too, users will be able to chat between devices. PubNub can also power the realtime interactions between players in your game, not just chat. This makes the possibilities endless with the PubNub API.

[![Build a Realtime Chatroom Lobby in Unity for iOS, Android and Web](https://i.ytimg.com/vi/AdLiDrBhPrY/hqdefault.jpg)](https://youtu.be/AdLiDrBhPrY)

## Step 1: Environment Setup
Let's up set up the environment first. The step is to import the PubNub SDK into your Unity project. You can do so by downloading the latest PubNub.unitypackage, then importing that package in Unity by going to **Assets -> Import Package -> Custom Package**.

Once imported, you may notice you have some errors in your console window. If you are getting error CS0234: The type or namespace name 'TestTools' does not exist, fix this problem by going to: **Window -> General -> Test Runner**. Then click on the drop-down menu in the top right corner and enable playmode tests for all assemblies.

![Enable Play Mode](https://unity.chat/media/enableplaymode-small.png)

*Note: If you download the GitHub repo, you won't have to re-import the PubNub package. You will only have to enable the Test Runner. Download that the repo <a href="https://github.com/JordanSchuetz/Unity-Realtime-Chat-Room-Tutorial">here</a>.*

![Screenshot](https://unity.chat/media/screenshot1.png)

The GitHub repo should include four folders. Assets, PubNub, Scenes and Scripts. In the Assets folder, there is a background image, submit button, font files and a loading circle. In the Scripts folder, there are the two scripts called SendMessage and LoadingCircle. The SendMessage script is where we will be writing the code to send and receive messages from clients in realtime.

## Finish the Tutorial by going to https://www.pubnub.com/docs/chat/tutorials/unity

Try the demo here. Open up two browser windows and zoom out in your browser window
https://pubnub.github.io/Unity-Realtime-Chat-Room-Tutorial/

## About PubNub

PubNub allows developers to build low-latency realtime applications that perform reliably and securely, at global scale. All you have to do is write the front end code with PubNub's easy to use API's, and PubNub handles all the back-end for you. This is important in the game development industry since if your game becomes more popular than you planned, you can risk your multiplayer servers having downtime.
