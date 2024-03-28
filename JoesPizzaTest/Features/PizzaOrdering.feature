Feature: JoesPizza

As a Customer
I want to order pizzas online

@mytag

Scenario: Selecting Pizza Type
Given I am on the Pizza Selection page
When I choose a "Margherita Pizza"
And I add it to my bag
Then the "Margherita Pizza" with quantity "1" should be in my bag

Scenario: Updating Quantity in Bag
Given I have added the "Margherita Pizza" to my bag
When I update the quantity of "Margherita Pizza" to "2"
Then the quantity of "Margherita Pizza" in my bag should be "2"

Scenario: Removing Item from the Bag
Given I have added the "Pepperoni Pizza" to my bag
When I remove the "Pepperoni Pizza" from my bag
Then the "Pepperoni Pizza" should not be in my bag

Scenario: Placing Order
Given I have added pizzas to my bag
When I place the order
Then I should see the order confirmation