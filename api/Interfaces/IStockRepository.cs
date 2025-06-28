using api.Dtos.Stock;
using api.Models;

namespace api.Interfaces;

public interface IStockRepository
{
  Task<List<Stock>> GetAllAsync();
  Task<Stock?> GetByIdAsync(int id); // "?" FirstOrDefault CAN BE NULL, that's why we need "?" to resolve errors from compiler
  Task<Stock> CreateAsync(Stock stockModel);
  Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto);
  Task<Stock?> DeleteAsync(int id);
  Task<bool> DeleteAllAsync();
}