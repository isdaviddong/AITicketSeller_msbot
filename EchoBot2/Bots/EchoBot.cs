﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.18.1

using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EchoBot2.Bots
{
    public class EchoBot : ActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var replyText = "";
            var UserMessage = turnContext.Activity.Text;
            if (UserMessage.Contains("/reset"))
            {
                ChatHistoryManager.DeleteIsolatedStorageFile();
                replyText = "我已經把之前的對談都給忘了!";
            }
            else
                 {
                var chatHistory = ChatHistoryManager.GetMessagesFromIsolatedStorage("UserA");
                replyText= ChatGPT.getResponseFromGPT(UserMessage, chatHistory);
                //儲存聊天紀錄
                ChatHistoryManager.SaveMessageToIsolatedStorage(
                    System.DateTime.Now, "UserA", UserMessage, replyText);
            }
              
            await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}
