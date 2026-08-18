export default {
  content: ['./index.html', './src/**/*.{js,jsx,ts,tsx}'],
  theme: {
    extend: {
      colors: {
        bg: {
          primary: '#07090f',
          secondary: '#0d1117',
          card: '#111827',
          elevated: '#161d2b',
          border: '#1e2a3a',
        },
        cyan: {
          glow: '#00e5ff',
          soft: '#00bcd4',
          dim: '#006070',
        },
        amber: {
          glow: '#ffab00',
        },
        red: {
          glow: '#ff3d57',
        },
        green: {
          glow: '#00e676',
        },
      },
      fontFamily: {
        display: ['Exo\\ 2', 'sans-serif'],
        mono: ['JetBrains\\ Mono', 'monospace'],
        body: ['Outfit', 'sans-serif'],
      },
      boxShadow: {
        'cyan-glow': '0 0 20px rgba(0, 229, 255, 0.15)',
        'cyan-glow-md': '0 0 40px rgba(0, 229, 255, 0.2)',
        'amber-glow': '0 0 20px rgba(255, 171, 0, 0.15)',
        'card': '0 4px 24px rgba(0, 0, 0, 0.4)',
      },
      animation: {
        'pulse-slow': 'pulse 3s cubic-bezier(0.4, 0, 0.6, 1) infinite',
        'fade-in': 'fadeIn 0.4s ease-out',
        'slide-up': 'slideUp 0.35s ease-out',
        'slide-in-right': 'slideInRight 0.3s ease-out',
      },
      keyframes: {
        fadeIn: {
          '0%': { opacity: '0' },
          '100%': { opacity: '1' },
        },
        slideUp: {
          '0%': { opacity: '0', transform: 'translateY(16px)' },
          '100%': { opacity: '1', transform: 'translateY(0)' },
        },
        slideInRight: {
          '0%': { opacity: '0', transform: 'translateX(20px)' },
          '100%': { opacity: '1', transform: 'translateX(0)' },
        },
      },
      backgroundImage: {
        'grid-pattern': `linear-gradient(rgba(0,229,255,0.03) 1px, transparent 1px),
                         linear-gradient(90deg, rgba(0,229,255,0.03) 1px, transparent 1px)`,
        'card-gradient': 'linear-gradient(135deg, #111827 0%, #0d1117 100%)',
      },
      backgroundSize: {
        'grid': '40px 40px',
      },
    },
  },
  plugins: [],
}
