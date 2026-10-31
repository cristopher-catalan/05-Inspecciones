namespace Inspecciones.UI;

public static class InspectionStyles
{
    public const string Css = """
    :root {
        --primary: #d97706;
        --primary-dark: #b45309;
        --primary-soft: #fef3c7;
        --bg: #f8f6f0;
        --panel: #ffffff;
        --border: #e7e2d6;
        --ink: #292524;
        --muted: #78716c;
        --success: #15803d;
        --danger: #dc2626;
        --radius: 10px;
        --shadow: 0 4px 6px -1px rgba(41, 37, 36, 0.08);
    }
    * { box-sizing: border-box; margin: 0; padding: 0; }
    body.app-inspections {
        font-family: 'Inter', -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif;
        background: var(--bg); color: var(--ink); line-height: 1.5; min-height: 100vh;
    }
    .top {
        display: flex; justify-content: space-between; align-items: center;
        background: #292a25; color: #fff; padding: 14px 28px; position: sticky; top: 0; z-index: 40;
    }
    .brand { display: flex; align-items: center; gap: 14px; }
    .mark {
        width: 42px; height: 42px; border-radius: 8px;
        background: #e0ab25; color: #292a25; font-weight: 800;
        display: grid; place-items: center; font-size: 20px;
    }
    .brand h1 { font-size: 17px; font-weight: 700; color: #fff; }
    .brand p { font-size: 12px; color: #c9c6b9; }
    .page { max-width: 1360px; margin: 24px auto 60px; padding: 0 24px; }

    /* Session Bar */
    .session-bar {
        background: var(--panel); border: 1px solid var(--border);
        border-radius: var(--radius); padding: 12px 18px;
        display: flex; align-items: center; justify-content: space-between;
        gap: 16px; margin-bottom: 24px; box-shadow: var(--shadow);
    }
    .login-controls { display: flex; align-items: center; gap: 10px; flex-wrap: wrap; }
    .input-group { display: flex; flex-direction: column; gap: 2px; }
    .input-group label { font-size: 10px; font-weight: 700; color: var(--muted); text-transform: uppercase; }
    .input-group select, .input-group input {
        padding: 7px 11px; border: 1px solid var(--border); border-radius: 7px;
        font-size: 13px; background: #fff;
    }
    .btn {
        padding: 8px 16px; border-radius: 8px; font-weight: 700; font-size: 13px;
        border: 0; cursor: pointer; display: inline-flex; align-items: center; gap: 6px;
        transition: all 0.15s ease;
    }
    .btn-primary { background: #e0ab25; color: #292a25; }
    .btn-primary:hover { background: #c69519; }
    .btn-outline { background: #fff; border: 1px solid var(--border); color: var(--ink); }
    .btn-outline:hover { background: #f5f0e4; }

    /* Hero Banner */
    .field-hero {
        background: #f2ce68; border-radius: var(--radius); padding: 26px 32px;
        display: flex; justify-content: space-between; align-items: center;
        margin-bottom: 20px; box-shadow: var(--shadow);
    }
    .field-hero small { font-size: 11px; font-weight: 800; letter-spacing: 0.12em; color: #57481b; }
    .field-hero h2 { font-size: 28px; font-weight: 800; color: #292a25; margin: 4px 0 6px; }
    .field-hero p { font-size: 14px; color: #4d493d; max-width: 580px; }
    .round-stat {
        width: 90px; height: 90px; border: 4px solid #fff5d4; border-radius: 50%;
        background: #292a25; color: #ffdb71; display: grid; place-content: center; text-align: center;
    }
    .round-stat strong { font-size: 26px; font-weight: 800; }
    .round-stat span { font-size: 10px; text-transform: uppercase; }

    /* Field Strip */
    .field-strip {
        display: flex; align-items: center; gap: 28px; background: #292a25; color: #fff;
        padding: 14px 22px; border-radius: var(--radius); margin-bottom: 24px; flex-wrap: wrap;
    }
    .field-strip div small { font-size: 9px; color: #c9c6b9; text-transform: uppercase; display: block; font-weight: 700; }
    .field-strip div strong { font-size: 22px; font-weight: 800; color: #f2cf68; display: block; }

    /* Main Grid */
    .main-grid { display: grid; grid-template-columns: 1.6fr 1fr; gap: 20px; }
    @media(max-width: 960px) { .main-grid { grid-template-columns: 1fr; } }

    .panel {
        background: var(--panel); border: 1px solid var(--border);
        border-radius: var(--radius); padding: 22px; box-shadow: var(--shadow);
    }
    .panel-header {
        display: flex; justify-content: space-between; align-items: center;
        margin-bottom: 16px; padding-bottom: 12px; border-bottom: 1px solid var(--border);
    }

    /* Checklist Questions */
    .question-card {
        background: #fdfbf7; border: 1px solid var(--border); border-radius: 8px;
        padding: 14px; margin-bottom: 12px;
    }
    .question-options { display: flex; gap: 8px; margin: 10px 0; }
    .opt-btn {
        flex: 1; padding: 7px; border-radius: 6px; font-size: 12px; font-weight: 700;
        border: 1px solid var(--border); background: #fff; cursor: pointer; text-align: center;
    }
    .opt-btn.active-conforme { background: #dcfce7; color: #15803d; border-color: #86efac; }
    .opt-btn.active-noconforme { background: #fee2e2; color: #dc2626; border-color: #fca5a5; }
    .opt-btn.active-na { background: #f1f5f9; color: #64748b; border-color: #cbd5e1; }

    /* Score meter */
    .score-meter {
        height: 12px; border-radius: 99px; background: #e5e7eb; overflow: hidden; margin: 8px 0;
    }
    .score-meter-fill { height: 100%; transition: width 0.3s ease, background 0.3s ease; }

    /* Badges */
    .tag { padding: 3px 8px; border-radius: 6px; font-size: 11px; font-weight: 700; }
    .tag-programada { background: #fef3c7; color: #b45309; }
    .tag-ejecucion { background: #e0f2fe; color: #0284c7; }
    .tag-revision { background: #fdf2f8; color: #be185d; }
    .tag-cerrada { background: #dcfce7; color: #15803d; }

    /* Modals & Drawer */
    .modal-overlay, .drawer-overlay {
        position: fixed; inset: 0; background: rgba(41, 42, 37, 0.6);
        backdrop-filter: blur(4px); z-index: 120; opacity: 0; pointer-events: none;
        transition: opacity 0.2s ease; display: grid; place-items: center; padding: 20px;
    }
    .modal-overlay.open, .drawer-overlay.open { opacity: 1; pointer-events: auto; }
    .modal {
        background: #fff; width: 100%; max-width: 500px; border-radius: var(--radius);
        box-shadow: 0 10px 30px rgba(0,0,0,0.2); overflow: hidden;
    }
    .modal-header { padding: 18px 24px; border-bottom: 1px solid var(--border); display: flex; justify-content: space-between; align-items: center; }
    .modal-body { padding: 20px 24px; display: flex; flex-direction: column; gap: 14px; }
    .modal-footer { padding: 14px 24px; border-top: 1px solid var(--border); background: #fdfbf7; display: flex; justify-content: flex-end; gap: 10px; }

    .drawer {
        position: fixed; top: 0; right: -600px; bottom: 0; width: 100%; max-width: 580px;
        background: #fff; box-shadow: -4px 0 25px rgba(0,0,0,0.15); z-index: 121;
        transition: right 0.25s ease; display: flex; flex-direction: column;
    }
    .drawer.open { right: 0; }
    .drawer-header { padding: 20px 24px; border-bottom: 1px solid var(--border); display: flex; justify-content: space-between; align-items: flex-start; }
    .drawer-body { padding: 24px; overflow-y: auto; flex: 1; display: flex; flex-direction: column; gap: 16px; }

    /* Toasts */
    .toast-container { position: fixed; bottom: 20px; right: 20px; z-index: 200; display: flex; flex-direction: column; gap: 8px; }
    .toast {
        background: #292a25; color: #fff; padding: 12px 18px; border-radius: 8px;
        font-size: 13px; font-weight: 600; display: flex; align-items: center; gap: 10px;
    }
    .toast.success { border-left: 4px solid #e0ab25; }
    .toast.error { border-left: 4px solid #ef4444; }
    """;
}
