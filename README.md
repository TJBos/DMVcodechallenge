# DMV Code Challenge

## Commit 1

Since I'm not too familiar with a .NET environment, I first made a solution in Node/JavaScript.
This can be found in the JSfiles directory.

The solution I used is pretty straight forward, however I can not _provably_ say this is the most optimal solution.
It is an intuitive solution but in the entire combinatorial space there might be a more optimal theoretical solution.

I was not sure if it was allowed to change the order of the Customers. Sorting based on duration has a slight optimization.

This solution sorts the tellers based on their multipliers, then loops over every customer. For each customer, if a
specialty type matches, the first teller of the list that matches (and so with the best multiplier) is assigned. If no match, the first
overall teller is assigned.

## Commit 2

I added the C# code and managed to build and run the program. I had to change some slashes in the paths to make it run on my Mac and
hopefully restored them.
