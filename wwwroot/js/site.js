document.addEventListener('DOMContentLoaded', () => {
    const revealTargets = document.querySelectorAll('.reveal-on-scroll');

    if (revealTargets.length && 'IntersectionObserver' in window) {
        const observer = new IntersectionObserver((entries) => {
            entries.forEach((entry) => {
                if (entry.isIntersecting) {
                    entry.target.classList.add('revealed');
                    observer.unobserve(entry.target);
                }
            });
        }, { threshold: 0.2 });

        revealTargets.forEach((target) => observer.observe(target));
    } else {
        revealTargets.forEach((target) => target.classList.add('revealed'));
    }

    const counters = document.querySelectorAll('[data-counter]');
    counters.forEach((counter) => {
        const target = Number(counter.getAttribute('data-counter'));
        if (Number.isNaN(target)) {
            return;
        }

        const duration = 900;
        const steps = 30;
        const increment = target / steps;
        let current = 0;

        const timer = setInterval(() => {
            current += increment;
            if (current >= target) {
                counter.textContent = target.toString();
                clearInterval(timer);
            } else {
                counter.textContent = Math.floor(current).toString();
            }
        }, duration / steps);
    });
});
