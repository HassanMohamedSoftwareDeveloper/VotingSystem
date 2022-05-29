using Sandbox;

Counter yes = new("Yes", 4);
Counter no = new("No", 4);
Counter maybe = new("Maybe", 4);
Counter hopefully = new("Hopefully", 4);

CounterManager manager = new(yes, no, maybe,hopefully);

manager.AnnounceWinner();