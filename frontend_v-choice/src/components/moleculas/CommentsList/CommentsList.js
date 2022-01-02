import React, { Component } from 'react'
import { List, ListItem, Typography } from '@material-ui/core'

import CommentTile from '../../card&tiles/CommentTile/CommentTile'
import CommentArea from '../../atoms/CommentArea/CommentArea'
import styles from './CommentsList.module.css'

export class CommentsList extends Component {
	constructor(props) {
		super(props);
		this.state = {
			comments: [],
			loading: true,
			pageNumber: 1,
			onPage: 3,
			totalCount: 0
		};
	}

	componentDidMount() {
		this.fetchComments(this.props.filmId);
	}

	async fetchComments(filmId) {
		fetch(`https://localhost:5001/api/Comment?PageNumber=${this.state.pageNumber}&OnPageCount=${this.state.onPage}&FilmId=${filmId}`)
			.then(response => response.json())
			.then(result => this.setState({ comments: result.items, loading: false, totalCount: result.totalCount }));
	}

	createComment = (comment) => {
		this.setState({ comments: [...this.state.comments, comment] })
	}

	updateComment = (updComment) => {
		let ind = this.state.comments.findIndex(c => c.Id === updComment.Id);
		this.state.comments[ind].text = updComment.text;
		this.setState({ comments: this.state.comments });
	}

	deleteComment = (commentId) => {
		let ind = this.state.comments.findIndex(c => c.Id === commentId);
		this.state.comments.splice(ind, 1);
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
												<CommentTile
													comment={comment}
													userEmail={this.props.userEmail}
													onUpdateMethod={this.updateComment}
													onDeleteMethod={this.deleteComment}
												/>
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
