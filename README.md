# .NET API

This is a package to work with Kafka using [Greyhound sidecar](https://github.com/wix/greyhound/blob/master/docs/non-jvm-languages.md) service.</br>
this package is allowing you to take advantage of greyhound capabilities:
* declarative API
* Parallel message handling
* And more
</br> 
</br> 
Read [Greyhound](https://github.com/wix/greyhound) docs to learn more

## Creating Greyhound instance

```csharp
string sidecar = "http://localhost:9000";
string local = "http://localhost:9001";
string topicName = "my-topic";
ILoggerFactory loggerFactory = ... // build your logger

using Greyhound gh = new GreyhoundBuilder(sidecar)
  .WithLogger(loggerFactory)
  .WithProducer()
  .WithConsumer(local)
  .WithConsumeTopic(topicName, "my-consumer-group", (s, r) => HandleTopic(topicName, r))
  .Build();
  
  //...
  
  private void HandleTopic(string topic, Record r)
  {
    loggerFactory.CreateLogger<Greyhound>()
        .LogInformation("handling topic `{topic}` with key=`{key}` Payload=`{Payload}` ", topic, r.Key, r.Payload);
  }
```

## Produce event
```csharp
    string topic = "my-topic-name";
    string payload = "{value: ...}";
    string key = "topic-key" // optional - for ordering
    Dictionary<string,string> headers = new ();
    gh.Produce(new ProduceRequest(topic, payload, key, headers));
```

## Consume event 
In your greyhound config add topics you wish to consume with handler
```csharp
    new GreyhoundBuilder(sidecar)
    .WithConsumer(local)
    .WithConsumeTopic("topic-1", "my-consumer-group", (s, r) => HandleTopic(topicName, r))
    .WithConsumeTopic("topic-2", "my-consumer-group", (s, r) => HandleTopic(topicName, r))
    .WithConsumeTopic("topic-2", "other-consumer-group", (s, r) => HandleTopic(topicName, r))
```

you can have as many topics as you need to as many consumer groups you need


## Create topics in Kafka
```csharp
    gh.CreateTopicsAsync(new CreateTopicsRequest(new Topic(topicName, partitions: 5)));
```