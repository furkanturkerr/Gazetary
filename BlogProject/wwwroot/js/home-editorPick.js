    (function () {
    const slider    = document.getElementById('epSlider');
    if (!slider) return;

    const slides    = slider.querySelectorAll('.ep-slide');
    const dots      = slider.querySelectorAll('.ep-dot');
    const prevBtn   = document.getElementById('epPrev');
    const nextBtn   = document.getElementById('epNext');
    const progressEl= document.getElementById('epProgress');

    if (!slides.length) return;

    const INTERVAL  = 5500;   // ms between slides
    const TICK      = 60;     // progress bar update interval ms

    let current     = 0;
    let autoTimer   = null;
    let progressTimer = null;
    let elapsed     = 0;

    /* ── Show a specific slide ── */
    function goTo(n) {
    slides[current].classList.remove('ep-slide--active');
    dots[current]?.classList.remove('ep-dot--active');

    current = (n + slides.length) % slides.length;

    slides[current].classList.add('ep-slide--active');
    dots[current]?.classList.add('ep-dot--active');

    resetProgress();
}

    /* ── Progress bar ── */
    function resetProgress() {
    elapsed = 0;
    if (progressEl) progressEl.style.width = '0%';
}

    function tickProgress() {
    elapsed += TICK;
    const pct = Math.min((elapsed / INTERVAL) * 100, 100);
    if (progressEl) progressEl.style.width = pct + '%';
}

    /* ── Auto-play ── */
    function startAuto() {
    stopAuto();
    autoTimer     = setInterval(() => goTo(current + 1), INTERVAL);
    progressTimer = setInterval(tickProgress, TICK);
}

    function stopAuto() {
    clearInterval(autoTimer);
    clearInterval(progressTimer);
}

    /* ── Events ── */
    prevBtn?.addEventListener('click', () => { goTo(current - 1); startAuto(); });
    nextBtn?.addEventListener('click', () => { goTo(current + 1); startAuto(); });

    dots.forEach((dot, i) =>
    dot.addEventListener('click', () => { goTo(i); startAuto(); })
    );

    /* Touch / swipe */
    let touchStartX = 0;
    slider.addEventListener('touchstart', e => { touchStartX = e.touches[0].clientX; }, { passive: true });
    slider.addEventListener('touchend', e => {
    const dx = e.changedTouches[0].clientX - touchStartX;
    if (Math.abs(dx) > 50) { goTo(current + (dx < 0 ? 1 : -1)); startAuto(); }
});

    /* Pause on hover */
    slider.addEventListener('mouseenter', stopAuto);
    slider.addEventListener('mouseleave', startAuto);

    /* ── Init ── */
    startAuto();
})();