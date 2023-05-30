# Для 4 лабораторної

## retry/timeout

При створенні користувача через POST запит на /api/users також створюється кошик через POST запит на ```/api/baskets``` <br />
Цей запит з ймовірністю 40% буде виконуватись 15с (в налаштуваннях VirtualService timeout = 10s). <br />
Тому, щоб протестувати retry\timeout, достатньо буде кілька разів надіслати POST реквест на ```/api/users``` з таким JSON (всі поля є обов'язковими) 
```
{
    "firstName": "f",
    "lastName": "l",
    "email": "email@mail.com",
    "phoneNumber": "1029387",
    "orderIds": []
}
```

## circuit breaker

Для "поломки" пода треба зробити GET запит на ```/api/store/break```<br />
Тоді один з подів store-service "зламається" і GET запити на ```/api/baskets/test-circuit-breaker```, який запитує дані з  ```/api/store``` буде отримувати 503, що має викликати Circuit Breaker, якщо буде 3 респонси з 5xx статусом за 1 хв