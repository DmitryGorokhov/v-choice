import React, { Component } from 'react'
import { List, ListItem, Typography } from '@material-ui/core'

import CommentTile from '../../card&tiles/CommentTile/CommentTile'
import CommentArea from '../../atoms/CommentArea/CommentArea'
import styles from './CommentsList.module.css'

export class CommentsList extends Component {
	constructor(props) {
		super(props);
		this.state = { comments: [], loading: true };
	}

	componentDidMount() {
		this.fetchComments(this.props.filmId);
	}

	async fetchComments(filmId) {
		fetch(`api/comments/${filmId}`)
			.then(response => response.json())
			.then(result => this.setState({ comments: result, loading: false }));
	}

	createComment = (comment) => {
		this.setState({ comments: [...this.state.comments, comment] })
	}

	updateComment = (updComment) => {
		console.log(updComment);
		let ind = this.state.comments.findIndex(c => c.Id === updComment.Id);
		this.state.comments[ind].Text = updComment.Text;
		this.setState({ comments: this.state.comments });
	}

	render() {
		return (
			<div>
				{
					this.state.loading
						? <Typography>Загрузка...</Typography>
						:
						<List className={styles.list}>
							{
								this.state.comments.length !== 0
									? this.state.comments.map(comment => {
										return (
											<ListItem className={styles.listItem} key={comment.Id}>
												<CommentTile comment={comment} userEmail={this.props.userEmail} onUpdateMethod={this.updateComment} />
											</ListItem>
										)
									})
									: <Typography variant='h5'>Пока нет комментариев</Typography>
							}
						</List>
				}

				{
					this.props.userEmail === null
						? <Typography variant='h6'>
							Авторизируйтесь, чтобы оставить свой комментарий
							</Typography>
						: <CommentArea filmId={this.props.filmId} typeMethod="create" method={this.createComment} />
				}
			</div>
		)
	}
}

export default CommentsList
