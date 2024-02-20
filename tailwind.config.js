/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ['./**/**/*.razor'],
  theme: {
      extend: {
          fontFamily: {
              'montserrat': ['Montserrat', 'sans-serif']
          },
          colors: {
              'hard-gray': '#212121'
          },
          animation: {
              swipe: 'swipe 2s infinite',
              slideIn: 'slideIn .5s ease-in-out'
          },
          keyframes: {
              swipe: {
                  '0%': { opacity: '0', transform: 'translateY(-100%)' },
                  '80%': { opacity: '1' },
                  '100%': { opacity: '0', transform: 'translateY(100%)' }
              },
              slideIn: {
                  '0%': { transform: 'translateX(200%)' },
                  '100%': { transform: 'translateX(0)' }
              }
          }
      },
  },
  plugins: [],
}

