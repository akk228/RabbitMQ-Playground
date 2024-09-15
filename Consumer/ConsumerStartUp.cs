/*
 * I will create here application that will create the amount of consumers that you specify,
 * and these consumers will be consuming messages in a round-robin way
 * meaning that they are gonna be receiving messages one after another in a circle manner.
 * Also, for the sake of being more realistic we will give every consumer its own connection
 */

using QueueSettings.Bindings;

var consumers = new List<Task>();
var greetings = new[] { GreetingType.Greeting, GreetingType.Farewell, GreetingType.InformalFarewell };
var userInput = new TaskCompletionSource();

consumers.Add(
 Consumer.Consumer.Run(
  0,
  greetings.Where( g => g == GreetingType.Greeting).ToList(),
  userInput));

consumers.Add(
 Consumer.Consumer.Run(
  1,
  greetings.Where( g => g == GreetingType.Farewell).ToList(),
  userInput));

consumers.Add(
 Consumer.Consumer.Run(
  2,
  greetings.Where( g => g != GreetingType.Greeting).ToList(),
  userInput));

Console.WriteLine("Press any key to exit...");
Console.ReadKey();
userInput.SetResult();
await Task.WhenAll(consumers);

