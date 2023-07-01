namespace LoLAPI.Models;

public class OpenAiRepository
{
    public List<Message> messages { get; set; }
}

public class Message
{
    public string content { get; set; }
}



