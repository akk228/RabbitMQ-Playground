/*
 * I will create here application that will create the amount of consumers that you specify,
 * and these consumers will be consuming messages in a round-robin way
 * meaning that they are gonna be receiving messages one after another in a circle manner.
 * Also, for the sake of being more realistic we will give every consumer its own connection
 */

var consumerCount = 2;

Console.WriteLine("Enter the number of consumers you wanna run : ");

try {
    consumerCount = int.Parse(Console.ReadLine());
}
catch (Exception e) {
}

var consumers = new Task[consumerCount];
var userInput = new TaskCompletionSource();

for (int i = 0; i < consumerCount; i++) {
    var delayInMs = 1000 + i * 2000;
    consumers[i] = Consumer.Consumer.Run(i, delayInMs, userInput);
}

Console.WriteLine("Press any key to exit...");
Console.ReadKey();
userInput.SetResult();
await Task.WhenAll(consumers);

