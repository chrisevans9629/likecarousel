using Microsoft.JSInterop;

namespace SwipeCard
{
    // This class provides an example of how JavaScript functionality can be wrapped
    // in a .NET class for easy consumption. The associated JavaScript module is
    // loaded on demand when first needed.
    //
    // This class can be registered as scoped DI service and then injected into Blazor
    // components for use.

    public class SwipeJsInterop : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        DotNetObjectReference<SwipeJsInterop> dotNetRef;

        public SwipeJsInterop(IJSRuntime jsRuntime)
        {
            dotNetRef = DotNetObjectReference.Create(this);
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/SwipeCard/swipejs.js").AsTask());
        }

        public async ValueTask<string> Prompt(string message)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<string>("showPrompt", message);
        }

        public async Task Start()
        {
            var module = await moduleTask.Value;

            await module.InvokeVoidAsync("Start", dotNetRef);
        }

        public event EventHandler<int>? OnSwipe;

        [JSInvokable]
        public void Swipe(int direction)
        {
            OnSwipe?.Invoke(this, direction);
        }

        public async Task CardAdded()
        {
            var module = await moduleTask.Value;

            await module.InvokeVoidAsync("CardAdded");
        }

        public async Task Handle()
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("Handle");
        }

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
                dotNetRef.Dispose();
            }
        }
    }
}