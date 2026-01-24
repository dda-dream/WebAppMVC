# WebAppMVC

Это проект веб-приложения, разработанного с использованием ASP.NET Core MVC.

## Структура проекта

- `WebAppMVC` - Основное веб-приложение
- `WebApp_Models` - Модели данных
- `WebApp_DataAccess` - Доступ к данным и репозитории
- `WEBAPP_Utility` - Вспомогательные классы и утилиты

## Настройка среды разработки

### Требования

- Visual Studio 2022 или новее
- .NET 6 SDK
- SQL Server LocalDB или SQL Server

### Установка

1. Откройте решение `WebAppMVC.sln` в Visual Studio
2. Восстановите пакеты NuGet
3. Обновите строку подключения к базе данных в `appsettings.json`
4. Запустите миграции Entity Framework:
   ```
   Update-Database
   ```

## GitHub Copilot

Проект включает настройки и руководства для использования GitHub Copilot:

- [Настройка Copilot](.vscode/settings.json)
- [Руководство по использованию Copilot](.vscode/copilot-guidelines.md)
- [Дополнительная информация](README-COPILOT.md)

Для эффективной работы с ИИ следуйте рекомендациям в этих файлах.

## Развертывание

Приложение может быть развернуто как на локальном сервере, так и в облаке с помощью Docker.

### Docker

Для сборки образа Docker выполните:

```
docker build -t webappmvc .
```

Для запуска контейнера:

```
docker run -d -p 8080:80 --name webappmvc-container webappmvc
```

## Лицензия

Этот проект лицензирован в соответствии с условиями лицензии MIT.