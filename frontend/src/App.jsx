import { useMemo, useState } from 'react'
import './App.css'
import ChatHeader from './components/ChatHeader.jsx'
import MessageList from './components/MessageList.jsx'
import MessageInput from './components/MessageInput.jsx'

const STUB_RESPONSES = [
  "Got it  this is a stubbed response while the real backend is being wired up.",
  'Imagine a smart agent replying here once your API is connected.',
  'This conversation is running entirely in your browser right now.',
  'You can customize these placeholder responses in src/App.jsx.',
]

const createInitialMessages = () => {
  const now = new Date()
  const minutesAgo = (n) => new Date(now.getTime() - n * 60_000)
  const format = (date) =>
    new Intl.DateTimeFormat('en-US', {
      hour: 'numeric',
      minute: '2-digit',
    }).format(date)

  return [
    {
      id: 1,
      sender: 'assistant',
      content: 'Welcome to the agent chat prototype. This entire conversation is stored only in memory.',
      timestamp: format(minutesAgo(3)),
    },
    {
      id: 2,
      sender: 'user',
      content: 'Nice. So none of this is actually calling a backend yet?',
      timestamp: format(minutesAgo(2)),
    },
    {
      id: 3,
      sender: 'assistant',
      content:
        'Correct. Messages live purely in React state right now. When your API is ready, you can persist or stream them from the server.',
      timestamp: format(minutesAgo(2)),
    },
    {
      id: 4,
      sender: 'user',
      content: 'And these bubbles and layout are just styled with CSS in App.css?',
      timestamp: format(minutesAgo(1)),
    },
    {
      id: 5,
      sender: 'assistant',
      content:
        'Exactly. The UI is intentionally simple: one conversation in memory, with components under src/components.',
      timestamp: format(minutesAgo(1)),
    },
  ]
}

function App() {
  const [messages, setMessages] = useState(createInitialMessages)
  const [isThinking, setIsThinking] = useState(false)
  const [responseIndex, setResponseIndex] = useState(0)

  const subtitle = useMemo(
    () => 'A minimal React front end for chat, with client-side stubbed responses.',
    [],
  )

  const handleSendMessage = (text) => {
    const trimmed = text.trim()
    if (!trimmed) return

    const timestamp = new Intl.DateTimeFormat('en-US', {
      hour: 'numeric',
      minute: '2-digit',
    }).format(new Date())

    setMessages((prev) => [
      ...prev,
      {
        id: prev.length + 1,
        sender: 'user',
        content: trimmed,
        timestamp,
      },
    ])

    setIsThinking(true)

    const currentIndex = responseIndex

    window.setTimeout(() => {
      setMessages((prev) => [
        ...prev,
        {
          id: prev.length + 1,
          sender: 'assistant',
          content: STUB_RESPONSES[currentIndex % STUB_RESPONSES.length],
          timestamp: new Intl.DateTimeFormat('en-US', {
            hour: 'numeric',
            minute: '2-digit',
          }).format(new Date()),
        },
      ])
      setIsThinking(false)
      setResponseIndex((value) => value + 1)
    }, 600)
  }

  return (
    <div className="app-shell">
      <div className="chat-shell" role="main">
        <aside className="chat-sidebar" aria-label="Conversation context">
          <div className="chat-sidebar__section">
            <h2 className="chat-sidebar__title">Today</h2>
            <p className="chat-sidebar__text">
              This is a single, local conversation. When your backend is ready,
              connect it here.
            </p>
          </div>
          <div className="chat-sidebar__section chat-sidebar__section--muted">
            <h3 className="chat-sidebar__subtitle">Implementation notes</h3>
            <ul className="chat-sidebar__list">
              <li>Pure React state – no server calls yet.</li>
              <li>Components live in src/components.</li>
              <li>Replace the timeout in App.jsx with a real API call.</li>
            </ul>
          </div>
        </aside>

        <section className="chat-main" aria-label="Chat">
          <ChatHeader subtitle={subtitle} />
          <MessageList messages={messages} />
          <MessageInput onSend={handleSendMessage} isThinking={isThinking} />
        </section>
      </div>
    </div>
  )
}

export default App
