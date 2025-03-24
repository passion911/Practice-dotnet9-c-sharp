

using AutoMapper;
using Infrastructure.DAL.Contracts;
using Infrastructure.DAL.Entities;

namespace Application.Services.OrderStuffs;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task PlaceOrderAsync(PlaceOrderDto orderDto)
    {
        try
        {
            // Map basic Order fields
            var order = _mapper.Map<Order>(orderDto);

            foreach (var itemDto in orderDto.Items)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(itemDto.ProductId);
                if (product is null)
                    throw new Exception($"Product with ID {itemDto.ProductId} not found.");

                if (product.Stock < itemDto.Quantity)
                    throw new Exception($"Insufficient stock for {product.ProductName}.");

                // Map OrderDetail from DTO
                var orderDetail = _mapper.Map<OrderDetail>(itemDto);
                orderDetail.Product = product;
                orderDetail.UnitPrice = product.Price; // snapshot current price

                order.OrderDetails.Add(orderDetail);

                // Update stock
                product.Stock -= itemDto.Quantity;
                await _unitOfWork.Products.UpdateAsync(product);
            }

            // Add order
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CompleteAsync();


        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
