import PropTypes from 'prop-types'

function ChatHeader({ subtitle }) {
  return (
    <header className="chat-header">
      <div className="chat-header__titles">
        <h1 className="chat-header__title">Agent Chat</h1>
        <p className="chat-header__subtitle">{subtitle}</p>
      </div>
      <div className="chat-header__status" aria-live="polite" aria-atomic="true">
        <span className="chat-header__badge chat-header__badge--accent">Prototype</span>
        <span className="chat-header__badge">Local only · Stubbed responses</span>
      </div>
    </header>
  )
}

ChatHeader.propTypes = {
  subtitle: PropTypes.string.isRequired,
}

export default ChatHeader
