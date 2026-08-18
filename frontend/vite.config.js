import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import path from 'path'
import { fileURLToPath } from 'url'

const __dirname = path.dirname(fileURLToPath(import.meta.url))

export default defineConfig({
  plugins: [react()],

  root: 'd:/projecys/game_dash_board_csharp/frontend',
  resolve: {

    preserveSymlinks: true,
  },
  server: {
    port: 3000,
    proxy: {
      '/api': {
        target: 'http://localhost:5149',
        changeOrigin: true,
      },
    },
  },
})
