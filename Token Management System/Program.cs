class ServiceToken
{
    public int TokenID { get; }
    public int Position { get; set; }
    public DateTime TicketDatetime { get; }
    public string? Status { get; set; }

    public ServiceToken(int position, DateTime ticketDatetime, string status)
    {
        //TokenID = new Random().Next(Int32.MaxValue / 2, Int32.MaxValue);
        TokenID = new Random().Next(1000, 9999);
        Position = position;
        TicketDatetime = ticketDatetime;
        Status = status;
    }
}

class TicketManager
{
    public Queue<ServiceToken> ServiceTickets { get; set; }
    LinkedList<ServiceToken> list = new LinkedList<ServiceToken>();
    LinkedList<ServiceToken> skippedTokenList = new LinkedList<ServiceToken>();

    //Constructor
    public TicketManager()
    {
        ServiceTickets = new Queue<ServiceToken>();
    }

    public void GenerateServiceToken()
    {
        int position  = ServiceTickets.Count + 1;
        DateTime ticketCreationTime = DateTime.Now;
        string status = "Pending";
        ServiceToken token = new ServiceToken(position, ticketCreationTime, status);
        ServiceTickets.Enqueue(token);
        Console.WriteLine("Service Token with token Id : " + token.TokenID + " is Created.\n");
    }

    public void GetNextToken()
    {
        if(ServiceTickets.Count > 0)
        {
            lock(ServiceTickets)
            {
                ServiceToken skippedToken = ServiceTickets.Peek();
                list.AddLast(skippedToken); // Removing Token from the List
                ServiceTickets.Dequeue(); // Adding Token to the List
                if(ServiceTickets.Count > 0)
                {
                    ServiceToken nextToken = ServiceTickets.Peek();
                    Console.WriteLine("Next token is "+nextToken.TokenID + " and its Position is "+nextToken.Position+"\n");
                }
            }
        }
        else if(list.Count > 0)
        {
            lock(list)
            {
                while(list.Count > 0)
                {
                    ServiceToken nextToken = list.First.Value;
                    if(nextToken.Status == "Complete")
                    {
                        list.RemoveFirst();
                    }
                    else
                    {
                        Console.WriteLine("Next token is " + nextToken.TokenID + " and its Position is " + nextToken.Position + "\n");
                        list.RemoveFirst();
                        list.AddLast(nextToken);
                        break;
                    }
                }
            }
        }
        else if (skippedTokenList.Count > 0)
        {
            lock (skippedTokenList)
            {
                while (skippedTokenList.Count > 0)
                {
                    ServiceToken nextToken = skippedTokenList.First.Value;
                    if (nextToken.Status == "Complete")
                    {
                        skippedTokenList.RemoveFirst();
                    }
                    else
                    {
                        Console.WriteLine("Next token is " + nextToken.TokenID + " and its Position is " + nextToken.Position + "\n");
                        skippedTokenList.RemoveFirst();
                        skippedTokenList.AddLast(nextToken);
                        break;
                    }
                }
            }
        }
        else
        {
            Console.WriteLine("No Token found in the Queue");
        }
    }



    public void UpdateToken(int tokenID)
    {
        if (tokenID > 0)
        {
            lock (ServiceTickets)
            {
                foreach (ServiceToken token in ServiceTickets)
                {
                    if (token.TokenID == tokenID)
                    {
                        token.Status = "Complete";
                        Console.WriteLine("Token with ID : " + token.TokenID + " updated successfully. Token Status : " + token.Status + "\n");
                        break;
                    }
                }
            }

            lock (skippedTokenList)
            {
                foreach (ServiceToken token in skippedTokenList)
                {
                    if (token.TokenID == tokenID)
                    {
                        token.Status = "Complete";
                        Console.WriteLine("Token with ID : " + token.TokenID + " updated successfully. Token Status : " + token.Status + "\n");
                        break;
                    }
                }
            }
            lock (list)
            {
                foreach (ServiceToken token in list)
                {
                    if (token.TokenID == tokenID)
                    {
                        token.Status = "Complete";
                        Console.WriteLine("Token with ID : " + token.TokenID + " updated successfully. Token Status : " + token.Status + "\n");
                        break;
                    }
                }
            }
        }
        else
        { 
        Console.WriteLine("Token Id " + tokenID + " not found.\n");
        }
    }

    public void SkipToken()
    {
        lock (ServiceTickets)
        {
            if (ServiceTickets.Count > 0)
            {
                ServiceToken skippedToken = ServiceTickets.Dequeue();
                skippedTokenList.AddLast(skippedToken);
                if(ServiceTickets.Count > 0)
                {
                    ServiceToken subsequentToken = ServiceTickets.Peek();
                    Console.WriteLine("Skipped Token : "+skippedToken.TokenID+" and Subsequent token : "+subsequentToken.TokenID + "\n");
                }
                else
                {
                    Console.WriteLine("No more tokens to display.\n");
                }
            }
            else if(skippedTokenList.Count > 0)
            {
                lock(skippedTokenList)
                {
                    while(skippedTokenList.Count > 0)
                    {
                        ServiceToken nextToken = skippedTokenList.First.Value;
                        if(nextToken.Status == "Complete")
                        {
                            skippedTokenList.RemoveFirst();
                        }
                        else
                        {
                            Console.WriteLine("Next token is "+ nextToken.TokenID+" and its Position is "+nextToken.Position+"\n");
                            skippedTokenList.RemoveFirst();
                            skippedTokenList.AddLast(nextToken);
                            break;
                        }
                    }
                }
            }
            else if (list.Count > 0)
            {
                lock (list)
                {
                    while (list.Count > 0)
                    {
                        ServiceToken nextToken = list.First.Value;
                        if (nextToken.Status == "Complete")
                        {
                            list.RemoveFirst();
                        }
                        else
                        {
                            Console.WriteLine("Next token is " + nextToken.TokenID + " and its Position is " + nextToken.Position + "\n");
                            list.RemoveFirst();
                            list.AddLast(nextToken);
                            break;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Cannot skip. Not enough tokens in queue.");
            }
        }
    }

    public void ListAllTokens()
    {
        if(ServiceTickets.Count > 0 || list.Count > 0 || skippedTokenList.Count > 0) 
        {
            Console.WriteLine("=====================List of all the tokens in the Queue================== : \n");
            if(list.Count > 0 || skippedTokenList.Count > 0)
            {
                Console.WriteLine("Tokens in the Linkedlist of skippedTokens\n");
                foreach(ServiceToken token in skippedTokenList)
                {
                    Console.WriteLine($"Token ID: {token.TokenID}, Position: {token.Position}, Ticket Datetime: {token.TicketDatetime}, Status: {token.Status}\n");
                }
                //=====================================================================
                Console.WriteLine("Tokens in the Linkedlist of GetNextToken\n");
                foreach (ServiceToken token in list)
                {
                    Console.WriteLine($"Token ID: {token.TokenID}, Position: {token.Position}, Ticket Datetime: {token.TicketDatetime}, Status: {token.Status}\n");
                }
                //=====================================================================
                Console.WriteLine("Tokens in Normal Queue\n");
                foreach (ServiceToken token in ServiceTickets)
                {
                    Console.WriteLine($"Token ID: {token.TokenID}, Position: {token.Position}, Ticket Datetime: {token.TicketDatetime}, Status: {token.Status}\n");
                }

            }
            else
            {
                foreach (ServiceToken token in ServiceTickets)
                {
                    Console.WriteLine($"Token ID: {token.TokenID}, Position: {token.Position}, Ticket Datetime: {token.TicketDatetime}, Status: {token.Status}");
                }
            }
        }
        else
        {
            Console.WriteLine("No Token available to display\n");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        TicketManager ticketManager = new TicketManager();
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\nTOKEN MANAGEMENT SYSTEM");
            Console.WriteLine("1. Create Token\n2. Get Next Token\n3. Update Token\n4. Skip Token\n5. List all tokens\n6. Exit");
            Console.Write("Enter your Choice: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Thread t1 = new Thread(new ThreadStart(ticketManager.GenerateServiceToken));
                    t1.Start();
                    t1.Join();
                    break;
                case "2":
                    Thread t2 = new Thread(new ThreadStart(ticketManager.GetNextToken));
                    t2.Start();
                    t2.Join();
                    break;
                case "3":
                    Console.Write("Enter token ID to update: ");
                    int updateTokenID = int.Parse(Console.ReadLine());
                    Thread t3 = new Thread(() => ticketManager.UpdateToken(updateTokenID));
                    t3.Start();
                    t3.Join();
                    break;
                case "4":
                    Thread t4 = new Thread(new ThreadStart(ticketManager.SkipToken));
                    t4.Start();
                    t4.Join();
                    break;
                case "5":
                    Thread t5 = new Thread(new ThreadStart(ticketManager.ListAllTokens));
                    t5.Start();
                    t5.Join();
                    break;
                case "6":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please enter a valid option.");
                    break;
            }
        }
    }
}
