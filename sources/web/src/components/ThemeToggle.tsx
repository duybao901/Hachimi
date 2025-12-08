// src/components/ThemeToggle.tsx
import React from 'react';
import { useTheme } from '../lib/theme-context';

export const ThemeToggle: React.FC = () => {
  const { theme, resolvedTheme, setTheme, toggle } = useTheme();

  return (
    <div style={{ display: 'flex', gap: 8, alignItems: 'center' }}>
      <button
        onClick={toggle}
        aria-label="Toggle theme"
        title={`Toggle theme (current: ${resolvedTheme})`}
        style={{
          padding: '6px 10px',
          borderRadius: 8,
          border: '1px solid rgba(0,0,0,0.06)',
          background: 'transparent',
          cursor: 'pointer'
        }}
      >
        {resolvedTheme === 'dark' ? '🌙' : '☀️'}
      </button>

      <select
        value={theme}
        onChange={(e) => setTheme(e.target.value as 'light' | 'dark' | 'system')}
        aria-label="Theme preference"
      >
        <option value="system">System</option>
        <option value="light">Light</option>
        <option value="dark">Dark</option>
      </select>
    </div>
  );
};
