# Buber Diner Api

### Login Request
```json
{
    "firstName": "Amy",
    "lastName": "Santiago",
    "email": "amysantiago@gmail.com"
}
```

### Login Response
```json
{
    "id": "419df4c5-caab-4215-83e6-f59bd273bee2",
    "firstName": "Amy",
    "lastName": "Santiago",
    "email": "amysantiago@gmail.com",
    "token": "hguy..ioi"
}
```

### Register Request
```json
{
    "firstName": "Amy",
    "lastName": "Santiago",
    "email": "amysantiago@gmail.com",
    "password": "amy123"
}
```

### Register Response
```js
200 OK
```
```json
{
    "id": "419df4c5-caab-4215-83e6-f59bd273bee2",
    "firstName": "Amy",
    "lastName": "Santiago",
    "email": "amysantiago@gmail.com",
    "token": "hguy..ioi"
}
```