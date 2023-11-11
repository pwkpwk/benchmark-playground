﻿namespace AmbientBytes.Algorithms;

public sealed class Andrew : IDataProcessor
{
    IEnumerable<Entity> IDataProcessor.ProcessData(List<Entity> input, long startAfterId, IDataProcessor.Order order, int count, List<Entity> existingData)
    {
        var response = order == IDataProcessor.Order.Ascending
            ? input.Where(x => x.Id > startAfterId).ToList()
            : input.Where(x => x.Id < startAfterId).ToList();

        response.RemoveAll(item => existingData.Any(x => item.Id == x.Id));

        response = order == IDataProcessor.Order.Ascending
            ? response.OrderBy(x => x.Id).Take(count).ToList()
            : response.OrderByDescending(x => x.Id).Take(count).ToList();
        
        existingData.AddRange(response);

        return existingData;
    }
}