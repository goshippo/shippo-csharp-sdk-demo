# Shippo C# SDK Demo



This is a sample project that demonstrates how to use the Shippo C# SDK.

This sample illustrates how to
* Create a Shipment object, including sender/recipient addresses and parcel details
* Collect shipping rates offered by the carriers configured in your Shippo account
* Use a selected rate to request a label
* Download the label for use in shipping your package

## Running the code

Before you run this code, you will need to have performed the below steps:
1. Install a recent version of [the .NET SDK](https://dotnet.microsoft.com/en-us/download) - this sample is using .NET 8
2. [Create a Shippo Account](https://apps.goshippo.com/join)
3. [Generate a Shippo API Token](https://support.goshippo.com/hc/en-us/articles/360026412791-Managing-Your-API-Tokens-in-Shippo#:~:text=Generate%20a%20Token,-To%20generate%20a&text=To%20generate%20a%20Test%20Token,and%20purchase%20test%20shipping%20labels.). Since this is a sample app, it is recommended that you generate a test token rather than a production (i.e., paid) token.

