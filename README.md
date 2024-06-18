# Token Management System

This is a simple console application to manage a queue of customer service requests in the banking domain.
The application creates and manages service tokens to handle customer requests efficiently.

## Features

1. Create a new service token.
2. Retrieve the next available service token.
3. Update the status of a service token to "Complete".
4. Skip the immediate next token and retrieve the subsequent token.
5. List all service tokens with their details.
6. Exit the application.

## Classes

### `ServiceToken`

This class represents a service token with the following properties:
- `TokenID`: An integer that is auto-generated.
- `Position`: The position of the token in the queue.
- `TicketDatetime`: The date and time when the token was created.
- `Status`: The status of the token (e.g., "Pending", "Complete").

### `TicketManager`

This class manages the queue of service tokens with the following methods:
- `GenerateServiceToken()`: Creates a new service token and adds it to the queue.
- `GetNextToken()`: Retrieves the next available token from the queue.
- `UpdateToken(int tokenID)`: Updates the status of the specified token to "Complete".
- `SkipToken()`: Skips the immediate next token and retrieves the subsequent token.
- `ListAllTokens()`: Lists all tokens in the queue with their details.

## Usage

Run the application and follow the on-screen instructions to manage the service tokens. The menu options are:
1. Create Token
2. Get Next Token
3. Update Token
4. Skip Token
5. List all tokens
6. Exit

##===================================================================


Select an option by entering the corresponding number. For example, enter `1` to create a new token.

## Multithreading

The application uses multithreading to handle operations concurrently, improving responsiveness.
Each operation runs in its own thread, allowing multiple operations to be executed simultaneously.

## Contributing

If you would like to contribute to this project, please fork the repository and create a pull request with your changes.

## License

This project is licensed by Gaurav Giradkar.



