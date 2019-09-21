Notes:
 - About line 17 in InventoryRepository.cs: 
 I started to write this code inside a Windows pc but continued on Linux one and ```GetCurrentDirectory()``` method works different on each platform and thats why i used ```Substring```. I don't want to publish ```db``` folder to ```bin``` directory.


Extra Id column in sales table:

Since there is no Id field for inventory-sales table i created one to make delete and update operations much easier.


select sl.Id,StoreName,ProductName,Date, SalesQuantity,Stock
                       from sales as sl inner join stores as st on sl.StoreId=st.Id 
                       inner join products as p on sl.ProductId=p.Id where SalesQuantity > 0;"
