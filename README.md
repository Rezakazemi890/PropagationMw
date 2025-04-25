# PropagationMw

A lightweight and safe alternative to ASP.NET Core's `HeaderPropagationHandler` for scenarios where HTTP context may not always be available — such as within `BackgroundService` or other non-request-based services.

This solution provides a custom `DelegatingHandler` (`SafeHeaderPropagationHandler`) that propagates specified headers when an active `HttpContext` exists and gracefully skips otherwise, preventing exceptions and duplicate client registrations.

---

## 📦 Features

- ✅ Safe header propagation from `HttpContext.Request` to outgoing `HttpRequestMessage`
- ✅ No exceptions when `HttpContext` is null (e.g., in `BackgroundService`)
- ✅ Supports propagating multiple custom headers
- ✅ Clean, dependency-free unit tests
- ✅ Simple integration with existing `HttpClient` registrations

---

## 🚀 Installation

Clone the repository:
```bash
git clone https://github.com/Rezakazemi890/PropagationMw.git
```

Or copy the `SafeHeaderPropagationHandler.cs` into your project.

---

## 🔧 Usage

1️⃣ Register `IHttpContextAccessor` in your `Program.cs`:

```csharp
services.AddHttpContextAccessor();
```

2️⃣ Add the handler to your `HttpClient` registration:

```csharp
services.AddHttpClient<IMyApiClient, MyApiClient>()
    .AddHttpMessageHandler(provider =>
    {
        var accessor = provider.GetRequiredService<IHttpContextAccessor>();
        var headersToPropagate = new[] { "X-Correlation-ID", "Authorization" };
        return new SafeHeaderPropagationHandler(accessor, headersToPropagate);
    });
```

---

## 📜 Example

If `HttpContext` is available:
- It reads and propagates the configured headers from the incoming request to the outgoing one.

If `HttpContext` is not available (e.g., in a `BackgroundService`):
- It skips the propagation silently without throwing exceptions.

---

## 🧪 Tests

Unit tests are included and can be run via:

```bash
dotnet test
```

✔ Tests include:
- Header propagation when `HttpContext` exists
- No propagation and no exceptions when `HttpContext` is null

No external dependencies like Moq are used — pure .NET and xUnit based.

---

## 📌 Why?

The built-in `HeaderPropagationHandler` in ASP.NET Core throws an `InvalidOperationException` when there's no active `HttpContext`, causing issues in background services and hosted processes.  
This library offers a clean, configurable, and safe way to handle such scenarios without duplicating `HttpClient` registrations or implementations.

---

## 📚 License

This project is open-sourced under the MIT License.

---

## 🙌 Contributing

Contributions and improvements are welcome. Feel free to fork and submit pull requests.

---

## 📎 Author

[Reza Kazemi](https://github.com/Rezakazemi890)
