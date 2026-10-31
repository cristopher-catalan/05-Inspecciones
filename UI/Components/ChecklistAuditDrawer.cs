namespace Inspecciones.UI.Components;

public static class ChecklistAuditDrawer
{
    public const string Html = """
    <div id="checklistDrawerOverlay" class="drawer-overlay" onclick="closeChecklistDrawer()"></div>
    <aside id="checklistDrawer" class="drawer">
        <div class="drawer-header">
            <div>
                <span id="drawerInspState" class="tag tag-programada"></span>
                <h3 id="drawerInspTitle" style="font-size:18px; font-weight:700; margin-top:4px;">Cuestionario de Campo</h3>
                <div style="font-size:12px; color:var(--muted); margin-top:2px;">
                    Sede: <strong id="drawerInspSite">—</strong> · Inspector: <span id="drawerInspInspector">—</span>
                </div>
            </div>
            <button type="button" class="btn btn-outline" onclick="closeChecklistDrawer()">✕</button>
        </div>
        <div class="drawer-body">
            <!-- Score Meter -->
            <section style="background:#fdfbf7; border:1px solid var(--border); border-radius:8px; padding:14px;">
                <div style="display:flex; justify-content:space-between; align-items:center;">
                    <span style="font-size:12px; font-weight:700; color:var(--muted);">PUNTAJE DE CONFORMIDAD</span>
                    <strong id="drawerInspScore" style="font-size:18px; color:var(--primary-dark);">0.00%</strong>
                </div>
                <div class="score-meter">
                    <div id="drawerScoreBar" class="score-meter-fill" style="width:0%; background:var(--primary);"></div>
                </div>
            </section>

            <!-- Questions Form -->
            <section>
                <div style="display:flex; justify-content:space-between; align-items:center; margin-bottom:12px;">
                    <span style="font-size:11px; font-weight:700; color:var(--muted); text-transform:uppercase;">Lista de Verificación</span>
                    <button type="button" class="btn btn-primary" id="btnSaveAnswers" onclick="saveActiveAnswers()" style="font-size:12px; padding:5px 12px;">
                        💾 Guardar Respuestas
                    </button>
                </div>
                <div id="checklistQuestionsContainer"></div>
            </section>

            <!-- Submit Section -->
            <section style="border-top:1px solid var(--border); padding-top:14px;" id="submitSection">
                <button type="button" class="btn btn-primary" onclick="handleSubmitInspection()" style="width:100%; justify-content:center; padding:12px;">
                    ✓ Finalizar y Enviar a Supervisión
                </button>
            </section>
        </div>
    </aside>
    """;

    public const string Script = """
    let activeQuestions = [];
    let currentAnswers = {}; // { questionId: { value, comment } }

    function renderChecklistQuestions(questions, answers) {
        activeQuestions = questions || [];
        currentAnswers = {};

        (answers || []).forEach(a => {
            currentAnswers[a.questionId] = { value: a.value, comment: a.comment || '' };
        });

        const container = byId('checklistQuestionsContainer');
        if (activeQuestions.length === 0) {
            container.innerHTML = '<div style="color:var(--muted); font-size:12px; padding:20px; text-align:center;">No hay preguntas en esta plantilla.</div>';
            return;
        }

        container.innerHTML = activeQuestions.map((q, idx) => {
            const cur = currentAnswers[q.id] || { value: '', comment: '' };
            return `
            <div class="question-card" id="qCard_${q.id}">
                <div style="display:flex; justify-content:space-between; align-items:flex-start;">
                    <strong style="font-size:13px; color:var(--ink);">${idx + 1}. ${esc(q.prompt)}</strong>
                    <span style="font-size:11px; color:var(--muted);">${q.points} pts</span>
                </div>
                <div class="question-options">
                    <button type="button" class="opt-btn ${cur.value === 'Conforme' ? 'active-conforme' : ''}" onclick="selectAnswer(${q.id}, 'Conforme')">✓ Conforme</button>
                    <button type="button" class="opt-btn ${cur.value === 'No conforme' ? 'active-noconforme' : ''}" onclick="selectAnswer(${q.id}, 'No conforme')">✗ No conforme</button>
                    <button type="button" class="opt-btn ${cur.value === 'No aplica' ? 'active-na' : ''}" onclick="selectAnswer(${q.id}, 'No aplica')">— No aplica</button>
                </div>
                <input type="text" id="qComment_${q.id}" value="${esc(cur.comment)}" placeholder="Comentario u observación de campo..." oninput="updateAnswerComment(${q.id}, this.value)" style="width:100%; padding:6px 10px; border:1px solid var(--border); border-radius:6px; font-size:12px;">
            </div>
            `;
        }).join('');

        updateScorePreview();
    }

    function selectAnswer(qId, val) {
        if (!currentAnswers[qId]) currentAnswers[qId] = { value: '', comment: '' };
        currentAnswers[qId].value = val;

        const card = byId(`qCard_${qId}`);
        if (card) {
            const btns = card.querySelectorAll('.opt-btn');
            btns[0].className = `opt-btn ${val === 'Conforme' ? 'active-conforme' : ''}`;
            btns[1].className = `opt-btn ${val === 'No conforme' ? 'active-noconforme' : ''}`;
            btns[2].className = `opt-btn ${val === 'No aplica' ? 'active-na' : ''}`;
        }

        updateScorePreview();
    }

    function updateAnswerComment(qId, c) {
        if (!currentAnswers[qId]) currentAnswers[qId] = { value: '', comment: '' };
        currentAnswers[qId].comment = c;
    }

    function updateScorePreview() {
        let total = 0;
        let earned = 0;
        activeQuestions.forEach(q => {
            if (q.required) {
                total += Number(q.points);
                const a = currentAnswers[q.id];
                if (a && a.value === 'Conforme') earned += Number(q.points);
            }
        });

        const pct = total === 0 ? 100 : Math.round(earned / total * 100);
        byId('drawerInspScore').textContent = `${pct}%`;
        const bar = byId('drawerScoreBar');
        bar.style.width = `${pct}%`;
        bar.style.background = pct >= 80 ? '#15803d' : (pct >= 50 ? '#d97706' : '#dc2626');
    }

    async function saveActiveAnswers() {
        if (!activeInspectionId) return;
        const answersList = activeQuestions.map(q => {
            const cur = currentAnswers[q.id] || { value: 'No aplica', comment: '' };
            return {
                questionId: q.id,
                value: cur.value || 'No aplica',
                comment: cur.comment || ''
            };
        });

        try {
            const r = await fetch(`/api/inspections/${activeInspectionId}/answers`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ answers: answersList })
            });
            if (r.ok) {
                showToast('Respuestas registradas en el servidor.', 'success');
                await loadInspectionsData();
            } else {
                const err = await r.json();
                showToast(err.error || 'Error al guardar respuestas.', 'error');
            }
        } catch {
            showToast('Fallo de conexión', 'error');
        }
    }

    async function handleSubmitInspection() {
        if (!activeInspectionId) return;
        await saveActiveAnswers();
        try {
            const r = await fetch(`/api/inspections/${activeInspectionId}/submit`, { method: 'POST' });
            if (r.ok) {
                showToast('¡Inspección finalizada y enviada a supervisión!', 'success');
                closeChecklistDrawer();
                await loadInspectionsData();
            } else {
                const err = await r.json();
                showToast(err.error || 'Faltan respuestas obligatorias.', 'error');
            }
        } catch {
            showToast('Fallo al enviar inspección', 'error');
        }
    }

    async function handleCloseFinding(findingId) {
        const comment = prompt('Acción correctiva implementada para cerrar este hallazgo:') || '';
        if (!comment.trim()) {
            showToast('Se requiere describir la acción correctiva.', 'error');
            return;
        }

        try {
            const r = await fetch(`/api/findings/${findingId}/close`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ comment })
            });
            if (r.ok) {
                showToast('Hallazgo cerrado con acción correctiva.', 'success');
                await loadInspectionsData();
            } else {
                const err = await r.json();
                showToast(err.error || 'Error al cerrar hallazgo.', 'error');
            }
        } catch {
            showToast('Fallo al conectar', 'error');
        }
    }
    """;
}
