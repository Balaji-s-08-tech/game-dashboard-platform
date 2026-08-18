import { useState, useCallback, useEffect, useRef } from 'react'

export function useAsync(asyncFn, immediate = false) {
  const [state, setState] = useState({
    data: null,
    loading: immediate,
    error: null,
  })

  const mountedRef = useRef(true)

  useEffect(() => {
    mountedRef.current = true
    return () => { mountedRef.current = false }
  }, [])

  const execute = useCallback(
    async (...args) => {
      setState((prev) => ({ ...prev, loading: true, error: null }))
      try {
        const result = await asyncFn(...args)
        if (mountedRef.current) {
          setState({ data: result, loading: false, error: null })
        }
        return { data: result, error: null }
      } catch (err) {
        const message = err.response?.data?.message || err.message || 'Something went wrong'
        if (mountedRef.current) {
          setState((prev) => ({ ...prev, loading: false, error: message }))
        }
        return { data: null, error: message }
      }
    },
    [asyncFn]
  )

  useEffect(() => {
    if (immediate) execute()
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [])

  const reset = useCallback(() => {
    setState({ data: null, loading: false, error: null })
  }, [])

  return { ...state, execute, reset }
}

export function useLocalStorage(key, initialValue) {
  const [value, setValue] = useState(() => {
    try {
      const stored = localStorage.getItem(key)
      return stored !== null ? JSON.parse(stored) : initialValue
    } catch {
      return initialValue
    }
  })

  const set = useCallback(
    (newValue) => {
      setValue(newValue)
      try {
        localStorage.setItem(key, JSON.stringify(newValue))
      } catch 
    },
    [key]
  )

  const remove = useCallback(() => {
    setValue(initialValue)
    localStorage.removeItem(key)
  }, [key, initialValue])

  return [value, set, remove]
}
