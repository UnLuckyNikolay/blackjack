# Blackjack

One of the first learning projects.  
A small task of building a very simple CLI blackjack (just random numbers 1-11 for two players) that went far beyond the original scope to include a proper deck, ASCII graphics, and full game rules.  

![Gameplay Example](https://imgur.com/cJ0tHp0.png)

## Install and Run

1. Install [.NET](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) 8.0 or higher.

2. Clone the repository:

    ```bash
	git clone https://github.com/UnLuckyNikolay/blackjack
    cd blackjack
	```

3. Build and run:

	```bash 
	dotnet build 
	dotnet run
	```

## Features

* 2-player game against the dealer
* ASCII card graphics
* A full deck of cards
* Win tracking across multiple rounds

## How to Play

* Enter your names.
* Wait for starting cards to get drawn.
* Choose if you want to draw additional cards.
* Try not to go over 21 points.

## Technical Features

* Single-file CLI application with no dependencies
* Game loop
* Dealer with its own logic
* Deck of cards saved as a list of IDs
* Proper Ace score handling logic (11 or 1, whichever is better)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.