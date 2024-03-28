using JoesPizza.Models;
using Microsoft.AspNetCore.Mvc;

namespace JoesPizza.Controllers
{
    public class OrderController : Controller
    {
        private static List<Pizza> pizzas = new List<Pizza>
        {
            new Pizza { Id = 1, Name = "Margherita Pizza", Price = 180 },
            new Pizza { Id = 2, Name = "Pepperoni Pizza", Price = 200 },
            new Pizza { Id = 3, Name = "Vegetarian Pizza", Price = 170 },
            new Pizza { Id = 4, Name = "Pepper Barbecue Chicken Pizza", Price = 220 },
            new Pizza { Id = 5, Name = "BBQ Chicken Pizza", Price = 240 },
            new Pizza { Id = 6, Name = "Paneer Tikka Pizza", Price = 210 },
            new Pizza { Id = 7, Name = "Cheese Burst Pizza", Price = 190 },
            new Pizza { Id = 8, Name = "Exotic Veg Pizza", Price = 220 },
        };

        private static List<OrderItem> orderItems = new List<OrderItem>();
        private static int orderIdCount = 246810;
        public IActionResult PizzaSelection()
        {
            return View(pizzas);
        }
        public List<Pizza> GetPizzas()
        {
            return pizzas;
        }
        public List<OrderItem> GetOrderItems()
        {
            return orderItems;
        }

        [HttpPost]
        public IActionResult AddToBag(int pizzaId)
        {
            var pizza = pizzas.FirstOrDefault(p => p.Id == pizzaId);
            if (pizza != null)
            {
                var existingItem = orderItems.FirstOrDefault(item => item.Pizza.Id == pizzaId);
                if (existingItem != null)
                {
                    existingItem.Quantity++;
                }
                else
                {
                    var orderItem = new OrderItem { Pizza = pizza, Quantity = 1 };
                    orderItems.Add(orderItem);
                }
                return RedirectToAction("PizzaSelection");
            }
            else
            {
                return NotFound();
            }
        }
        public IActionResult OrderCheckout()
        {
            return View(orderItems);
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int pizzaId, int quantity)
        {
            var existingItem = orderItems.FirstOrDefault(item => item.Pizza.Id == pizzaId);
            if (existingItem != null)
            {
                existingItem.Quantity = quantity;
            }
            return RedirectToAction("OrderCheckout");
        }

        [HttpPost]
        public IActionResult RemoveItem(int pizzaId)
        {
            var itemToRemove = orderItems.FirstOrDefault(item => item.Pizza.Id == pizzaId);
            if (itemToRemove != null)
            {
                orderItems.Remove(itemToRemove);
            }
            return RedirectToAction("OrderCheckout");
        }

        [HttpPost]
        public IActionResult PlaceOrder()
        {
            int orderId = orderIdCount++;

            decimal totalPrice = orderItems.Sum(item => item.Pizza.Price * item.Quantity);

            Order order = new Order
            {
                Id = orderId,
                Items = orderItems.Select(item => new OrderItem { Pizza = item.Pizza, Quantity = item.Quantity }).ToList(),
                TotalPrice = totalPrice
            };
            orderItems.Clear();
            return RedirectToAction("OrderConfirmation", order);
        }
        public IActionResult OrderConfirmation(Order placedOrder)
        {
            return View(placedOrder);
        }
    }
}
