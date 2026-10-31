namespace Inspecciones.UI.Components;

public static class ToastSystem
{
    public const string Html = """
    <div id="toastContainer" class="toast-container"></div>
    """;

    public const string Script = """
    function showToast(message, type = 'success') {
        const container = byId('toastContainer');
        if (!container) return;
        const toast = document.createElement('div');
        toast.className = `toast ${type}`;
        const icon = type === 'success' ? '✓' : (type === 'error' ? '✕' : 'ℹ');
        toast.innerHTML = `<span>${icon}</span> <span>${esc(message)}</span>`;
        container.appendChild(toast);
        setTimeout(() => {
            toast.style.opacity = '0';
            toast.style.transform = 'translateY(10px)';
            toast.style.transition = 'all 0.3s ease';
            setTimeout(() => toast.remove(), 300);
        }, 3500);
    }
    """;
}
