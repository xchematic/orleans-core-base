using System;
using System.Dynamic;
using System.Threading.Tasks;
using interfaces;
using Orleans;
using classes;

namespace Name
{
    class AgentGrain: Grain, IAgent
    {
        private IConversation Conversation;
        private AgentProperties AgentProperties;
        public override async Task OnActivateAsync()
        {
            if(AgentProperties != null){
                Conversation = this.GrainFactory.GetGrain<IConversation>(AgentProperties.ConversationKey);
                await Conversation.SetProperties(new ConversationProperties{
                    ConversationPass = AgentProperties.ConversationPass,
                    ConversationWorkspace = AgentProperties.ConversationWorkspace
                });
            }
            await base.OnActivateAsync();
        }
        public async Task SetProperties(AgentProperties properties){
            AgentProperties = properties;
            await OnActivateAsync();
        }
        public Task<AgentProperties> GetProperties(){
            return Task.FromResult(AgentProperties);
        }
        public async Task<MessageResult> SendMessage(string message)
        {
            if(Conversation != null)
            {
                return await Conversation.Message(message);
            }
            MessageResult errorMessage = new MessageResult();
            errorMessage.Success = false;
            errorMessage.Output = "I can't access my Rules Engine process";
            return errorMessage;
        }
    }
}