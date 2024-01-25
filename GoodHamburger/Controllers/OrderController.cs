using GoodHamburger.DTO;
using GoodHamburger.Models;
using GoodHamburger.Repositories;
using GoodHamburger.Repositories.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ISandwichRepository _sandwichRepository;
        private readonly IExtraRepository _extraRepository;

        public OrderController(IOrderRepository orderRepository, ISandwichRepository sandwichRepository, IExtraRepository extraRepository)
        {
            _orderRepository = orderRepository;
            _sandwichRepository = sandwichRepository;
            _extraRepository = extraRepository;
        }

        [HttpPost("CreateOrder")]
        public async Task<ActionResult<EndOrderDTO>> CreateOrder([FromBody] OrderDTO orderDto)
        {
            try
            {
                var extraorder = new ExtraEndDTO();
                var endOrder = new EndOrderDTO();
                endOrder.Extras = new List<ExtraEndDTO>();
                
                decimal amount = 0;
                decimal amountwithdiscound = 0;

                // Verificar se o pedido tem um sandwich válido

                SandwichModel sandwich = await _sandwichRepository.SearchSandwichById(orderDto.SandwichID);
                if (orderDto.SandwichID != null)
                {
                    if (sandwich == null)
                    {
                        return BadRequest("Sandwich ot found!");
                    }
                    amount += sandwich.Price;
                }

                //Verificar se não há extras repetidos no pedido

                var duplicateExtras = orderDto.ExtrasID
                .GroupBy(extra => extra)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key)
                .ToList();

                if (duplicateExtras.Any())
                {
                    return BadRequest("Duplicate Extras are not allowed");
                }


                var orderModel = new OrderModel();
                orderModel.SandwichId = orderDto.SandwichID;
                orderModel.Extras = new List<ExtraModel>();



                foreach (var item in orderDto.ExtrasID)
                {
                    var extraRepositori = await _extraRepository.SearchExtraById(item);

                    if (extraRepositori == null && item > 0)
                    {
                        return BadRequest($"Extra with ID {item} not found");
                    }

                   

                    if (extraRepositori != null)
                    {
                        var extraorderDTO = new ExtraEndDTO
                        {
                            Id = extraRepositori.Id,
                            Name = extraRepositori.Name,
                            Price = extraRepositori.Price
                        };

                        endOrder.Extras.Add(extraorderDTO);


                        var extramodel = new ExtraModel
                        {
                            Name = extraorderDTO.Name,
                            Price = extraorderDTO.Price
                        };


                            orderModel.Extras.Add(extramodel);
                        

                        if (extraorderDTO.Id == 1)
                        {
                            amount += extraorderDTO.Price;
                            amountwithdiscound = amount * 0.9M;
                        }
                        else if (extraorderDTO.Id == 2)
                        {
                            amount += extraorderDTO.Price;
                            amountwithdiscound = amount * 0.85M;
                        }
                    }
                }

                bool hasExtra1 = orderDto.ExtrasID.Contains(1);
                bool hasExtra2 = orderDto.ExtrasID.Contains(2);

                if (hasExtra1 && hasExtra2)
                {
                    amountwithdiscound = amount * 0.8M;
                }

                orderModel.Total = amountwithdiscound > 0 ? amountwithdiscound : amount;

                var result = await _orderRepository.CreateOrder(orderModel);

                endOrder.OrderID = result.Id;
                endOrder.Sandwich = sandwich;
                endOrder.Total = result.Total;
                return Ok(endOrder);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("SearchOrderById")]
        public async Task<ActionResult<EndOrderDTO>> SearchOrderById(int orderId)
        {
            var order = await _orderRepository.SearchOrderById(orderId);
            var sandwich = await _sandwichRepository.SearchSandwichById(order.SandwichId);
            order.Sandwich = sandwich;
            return Ok(order);
        }


        [HttpGet("SearchAllOrders")]
        public async Task<ActionResult<EndOrderDTO>> SearchAllOrder()
        {
            List<OrderModel> ordersRepository = await _orderRepository.SearchAllOrders();
            var ordersfinals = new List<EndOrderDTO>();

            foreach (var order in ordersRepository)
            {
                var endOrder = new EndOrderDTO();
                var extrasInOrder = await _extraRepository.SearchExtrasInOrderById(order.Id);
                var extraEndDTO = new List<ExtraEndDTO>();

                foreach (var extrasId in extrasInOrder)
                {
                    var extrasRepository = await _extraRepository.SearchExtraById(extrasId);
                    extraEndDTO.Add(extrasRepository);
                }

                endOrder.OrderID = order.Id;
                endOrder.Sandwich = await _sandwichRepository.SearchSandwichById(order.SandwichId);
                endOrder.Total = order.Total;
                endOrder.Extras = extraEndDTO;

                ordersfinals.Add(endOrder);
                    
            }


            return Ok(ordersfinals);
            
        }


        [HttpPut("UpdateOrderById")]
        public async Task<ActionResult<EndOrderDTO>> UpdateOrder(int orderId, int? sandwichId, List<int> extraIds)
        {
            try
            {
                decimal amount = 0;
                decimal amountWithDiscount = 0;

                var existingOrder = await _orderRepository.SearchOrderById(orderId);

                if (existingOrder == null)
                {
                    return NotFound($"Order with ID {orderId} not found.");
                }

                if (sandwichId.HasValue)
                {
                    existingOrder.SandwichId = sandwichId.Value;

                    SandwichModel newSandwich = await _sandwichRepository.SearchSandwichById(sandwichId.Value);
                    if (newSandwich != null)
                    {
                        amount += newSandwich.Price;
                    }
                    else
                    {
                        return NotFound($"Sandwich with ID {sandwichId.Value} not found.");
                    }
                }

                existingOrder.Extras?.Clear();

                // 4. Adicionar os novos extras (se houver)
                foreach (var extraId in extraIds)
                {
                    if (extraId <= 0)
                    {
                        // ID inválido, pular para a próxima iteração
                        continue;
                    }
                    // Verifique se o Extra existe antes de adicioná-lo
                    var extraRepository = await _extraRepository.SearchExtraById(extraId);
                    if (extraRepository != null)
                    {
                        ExtraModel extraModel = new ExtraModel
                        {
                            Name = extraRepository.Name,
                            Price = extraRepository.Price
                        };

                        existingOrder.Extras ??= new List<ExtraModel>();
                        existingOrder.Extras.Add(extraModel);

                        // Calcule o total com desconto, se necessário
                        if (extraId == 1)
                        {
                            amount += extraModel.Price;
                            amountWithDiscount = amount * 0.9M;
                        }
                        else if (extraId == 2)
                        {
                            amount += extraModel.Price;
                            amountWithDiscount = amount * 0.85M;
                        }
                    }
                    else
                    {
                        return NotFound($"Extra with ID {extraId} not found.");
                    }
                }

                // 5. Atualizar a ordem no repositório
                var updatedOrder = await _orderRepository.UpdateOrder(existingOrder);

                // 6. Construir o DTO de resposta
                EndOrderDTO orderDTO = new EndOrderDTO
                {
                    OrderID = updatedOrder.Id,
                    Sandwich = updatedOrder.Sandwich,
                    Extras = updatedOrder.Extras?.Select(extra => new ExtraEndDTO
                    {
                        Id = extra.Id,
                        Name = extra.Name,
                        Price = extra.Price
                    }).ToList(),
                    Total = amountWithDiscount > 0 ? amountWithDiscount : amount
                };

                return Ok(orderDTO);
            }
            catch (Exception ex)
            {
                // Lidar com exceções, se necessário
                return StatusCode(500, "Erro interno do servidor");
            }
        }





        //[HttpPut("UpdateOrderById")]
        //public async Task<ActionResult<EndOrderDTO>> UpdateOrder(int orderId, int? sandwichId, List<int>? extraId)
        //{
        //    decimal amount = 0;
        //    decimal amountwithdiscound = 0;

        //    // 1. Recuperar a ordem existente pelo ID
        //    var existingOrder = await _orderRepository.SearchOrderById(orderId);

        //    if (existingOrder == null)
        //    {
        //        return NotFound($"Order with ID {orderId} not found.");
        //    }

        //    // 2. Atualizar os campos desejados
        //    if (sandwichId.HasValue)
        //    {
        //        existingOrder.SandwichId = sandwichId.Value;

        //        SandwichModel newSandwich = new SandwichModel();
        //        newSandwich = await _sandwichRepository.SearchSandwichById(sandwichId.Value);
        //        amount += newSandwich.Price;

        //    }

        //    if (extraId.HasValue)
        //    {

        //        if (existingOrder.Extras == null || !existingOrder.Extras.Any(e => e.Id == extraId.Value))
        //        {

        //            existingOrder.Extras = new List<ExtraModel>();
        //            existingOrder.Extras.Clear();

        //            // Verifique se o Extra existe antes de adicioná-lo
        //            var extraRepository = await _extraRepository.SearchExtraById(extraId.Value);
        //            if (extraRepository != null)
        //            {

        //                ExtraModel extraModel = new ExtraModel();
        //                extraModel.Name = extraRepository.Name;
        //                extraModel.Price = extraRepository.Price;
        //                // Se não estiver associado, adicione-o
        //                existingOrder.Extras.Add(extraModel);
        //            }
        //            else
        //            {
        //                return NotFound($"Extra with ID {extraId.Value} not found.");
        //            }
        //        }

        //    }



        //    var order = await _orderRepository.UpdateOrder(existingOrder);

        //    EndOrderDTO orderDTO = new EndOrderDTO();
        //    var extraDTOList = new List<ExtraEndDTO>();

        //    foreach(var extra in order.Extras)
        //    {
        //    ExtraEndDTO extraDTO = new ExtraEndDTO();

        //        extraDTO.Id = extra.Id;
        //        extraDTO.Name = extra.Name;
        //        extraDTO.Price = extra.Price;
        //        extraDTOList.Add(extraDTO);

        //        if (extraDTO.Id == 1)
        //        {
        //            amount += extraDTO.Price;
        //            amountwithdiscound = amount * 0.9M;
        //        }
        //        else if (extraDTO.Id == 2)
        //        {
        //            amount += extraDTO.Price;
        //            amountwithdiscound = amount * 0.85M;
        //        }

        //    }
        //    orderDTO.Extras = extraDTOList;

        //    bool hasExtra1 = orderDTO.Extras.Any(extraDTO => extraDTO.Id == 1);
        //    bool hasExtra2 = orderDTO.Extras.Any(extraDTO => extraDTO.Id == 2);


        //    if (hasExtra1 && hasExtra2)
        //    {
        //        amountwithdiscound = amount * 0.8M;
        //    }



        //    orderDTO.OrderID = order.Id;
        //    orderDTO.Sandwich = order.Sandwich;

        //    orderDTO.Total = amountwithdiscound > 0 ? amountwithdiscound : amount;



        //    return orderDTO;
        //}

        [HttpDelete("ExcludeOrderById")]
        public async Task<bool> RemoveOrder(int orderId)
        {
            bool removed= await _orderRepository.RemoveOrder(orderId);
            return removed;
        }

    }
 }


