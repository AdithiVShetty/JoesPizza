using JoesPizza.Controllers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace JoesPizzaTest.StepDefinitions
{
    [Binding]
    public sealed class PizzaOrderingStepDefinitions
    {
        private readonly OrderController _orderController;
        private IActionResult _actionResult;

        public PizzaOrderingStepDefinitions()
        {
            _orderController = new OrderController();
        }

        [Given(@"I am on the Pizza Selection page")]
        public void GivenIAmOnThePizzaSelectionPage()
        {
            _actionResult = _orderController.PizzaSelection();
            Assert.IsInstanceOf<ViewResult>(_actionResult);
        }

        [When(@"I choose a ""(.*)""")]
        public void WhenIChooseAPizza(string pizzaName)
        {
            var pizza = _orderController.GetPizzas().FirstOrDefault(p => p.Name == pizzaName);
            Assert.IsNotNull(pizza);

            _actionResult = _orderController.AddToBag(pizza.Id);
            Assert.IsInstanceOf<RedirectToActionResult>(_actionResult);
        }

        [When(@"I add it to my bag")]
        public void WhenIAddItToMyBag()
        {
            // No specific action required here as adding to bag is handled in previous step
        }

        [Then(@"the ""(.*)"" with quantity ""(.*)"" should be in my bag")]
        public void ThenThePizzaWithQuantityShouldBeInMyBag(string pizzaName, int quantity)
        {
            var orderItems = _orderController.GetOrderItems();
            var pizza = orderItems.FirstOrDefault(item => item.Pizza.Name == pizzaName);
            Assert.IsNotNull(pizza);
            Assert.AreEqual(quantity, pizza.Quantity);
        }

        [Given(@"I have added the ""(.*)"" to my bag")]
        public void GivenIHaveAddedThePizzaToMyBag(string pizzaName)
        {
            WhenIChooseAPizza(pizzaName);
            WhenIAddItToMyBag();
        }

        [When(@"I update the quantity of ""(.*)"" to ""(.*)""")]
        public void WhenIUpdateTheQuantityOfPizzaTo(string pizzaName, int quantity)
        {
            var orderItems = _orderController.GetOrderItems();
            var pizza = orderItems.FirstOrDefault(item => item.Pizza.Name == pizzaName);
            Assert.IsNotNull(pizza);

            _actionResult = _orderController.UpdateQuantity(pizza.Pizza.Id, quantity);
            Assert.IsInstanceOf<RedirectToActionResult>(_actionResult);
        }

        [Then(@"the quantity of ""(.*)"" in my bag should be ""(.*)""")]
        public void ThenTheQuantityOfPizzaInMyBagShouldBe(string pizzaName, int quantity)
        {
            var orderItems = _orderController.GetOrderItems();
            var pizza = orderItems.FirstOrDefault(item => item.Pizza.Name == pizzaName);
            Assert.IsNotNull(pizza);
            Assert.AreEqual(quantity, pizza.Quantity);
        }

        [Given(@"I have added the ""(.*)"" with quantity ""(.*)"" to my bag")]
        public void GivenIHaveAddedThePizzaWithQuantityToMyBag(string pizzaName, int quantity)
        {
            WhenIChooseAPizza(pizzaName);
            WhenIAddItToMyBag();
            WhenIUpdateTheQuantityOfPizzaTo(pizzaName, quantity);
        }

        [When(@"I remove the ""(.*)"" from my bag")]
        public void WhenIRemoveThePizzaFromMyBag(string pizzaName)
        {
            var orderItems = _orderController.GetOrderItems();
            var pizza = orderItems.FirstOrDefault(item => item.Pizza.Name == pizzaName);
            Assert.IsNotNull(pizza);

            _actionResult = _orderController.RemoveItem(pizza.Pizza.Id);
            Assert.IsInstanceOf<RedirectToActionResult>(_actionResult);
        }

        [Then(@"the ""(.*)"" should not be in my bag")]
        public void ThenThePizzaShouldNotBeInMyBag(string pizzaName)
        {
            var orderItems = _orderController.GetOrderItems();
            var pizza = orderItems.FirstOrDefault(item => item.Pizza.Name == pizzaName);
            Assert.IsNull(pizza);
        }

        [Given(@"I have added pizzas to my bag")]
        public void GivenIHaveAddedPizzasToMyBag()
        {
            WhenIChooseAPizza("Margherita Pizza");
            WhenIAddItToMyBag();
            WhenIChooseAPizza("Pepperoni Pizza");
            WhenIAddItToMyBag();
        }

        [When(@"I place the order")]
        public void WhenIPlaceTheOrder()
        {
            _actionResult = _orderController.PlaceOrder();
            Assert.IsInstanceOf<RedirectToActionResult>(_actionResult);
        }

        [Then(@"I should see the order confirmation")]
        public void ThenIShouldSeeTheOrderConfirmation()
        {
            var result = _actionResult as RedirectToActionResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("OrderConfirmation", result.ActionName);
        }
    }
}
